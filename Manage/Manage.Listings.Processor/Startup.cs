using System;
using GreenPipes;
using Hangfire;
using Manage.Listings.Processor.Messages;
using Manage.Listings.Processor.Services;
using Manage.Listings.Processor.Services.Interfaces;
using Manage.Repository.Data;
using MassTransit;
using MessageBus;
using MessageBus.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Manage.Listings.Processor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region MySQL

            services.AddDbContext<ManageDataContext>(options =>
            {
                //options.UseLazyLoadingProxies();
                options.UseMySql(Configuration.GetConnectionString("ManageConnection"));
            });

            // services.AddDbContextPool<DataContext>(options => options
            //     // replace with your connection string
            //     .UseMySql(Configuration.GetConnectionString("DefaultConnection"), mySqlOptions => mySqlOptions
            //         // replace with your Server Version and Type
            //         .ServerVersion(new Version(8, 0, 18), ServerType.MySql)
            //     ));
            #endregion

            #region HangFire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();
            #endregion

            #region Services

            services.AddScoped<ICronService, CronService>();
            services.AddScoped<IListingProcessor, ListingProcessor>();
            #endregion
            

            #region MessageBus

            services.AddMassTransit(
                c =>
                {
                    c.AddConsumer<ListingPublishEventConsumer>();
                });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(Configuration["EventBus:HostName"], "/", h =>
                {
                    h.Username(Configuration["EventBus:UserName"]);
                    h.Password(Configuration["EventBus:Password"]);
                });

                cfg.ReceiveEndpoint(EventBusConstants.TestPublishQueue, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                    e.Consumer<ListingPublishEventConsumer>(provider);

                });


                cfg.ConfigureEndpoints(provider);

            }));
            services.AddSingleton<IHostedService, BusService>();

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ICronService cronService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();

            app.UseAuthentication();

            app.UseMvc();

            cronService.SetupConversion();

        }
    }
}
