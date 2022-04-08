using System;
using GreenPipes;
using MassTransit;
using MessageBus;
using MessageBus.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Search.Common.Interfaces;
using Search.Common.Models;
using Search.Elastic.Service.Messages;
using Search.Elastic.Service.Services;
using Search.Elastic.Service.Services.Interfaces;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Search.Elastic.Service
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

            
            #region Services
            
            services.AddScoped<IListingsService, ListingsService>();
            services.AddScoped<IPostCodesService, PostCodesService>();
            services.AddScoped<ILookupsService, LookupsService>();
            
            #endregion
            
            #region Redis
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration =
                    Configuration.GetConnectionString("Redis");
            });
            #endregion
            
            #region CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOriginsHeadersAndMethods",
                    policy => policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials());
                        
            });

            // .WithOrigins("http://localhost:3000","http://192.168.1.50:8080","http://192.168.1.50:3000")
            #endregion

            #region MessageBus

            services.AddMassTransit(
                c =>
                {
                    c.AddConsumer<ListingPublishCommandConsumer>();
                } );

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(Configuration["EventBus:HostName"], "/", h =>
                {
                    h.Username(Configuration["EventBus:UserName"]);
                    h.Password(Configuration["EventBus:Password"]);
                });
                
                cfg.ReceiveEndpoint(EventBusConstants.PublishListingToElasticCommandQueue, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                    e.Consumer<ListingPublishCommandConsumer>(provider);

                });
                

                cfg.ConfigureEndpoints(provider);
                
            }));
            services.AddSingleton<IHostedService, BusService>();

            #endregion
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOriginsHeadersAndMethods");
            
            app.UseAuthentication();

            app.UseMvc();

        }
    }
}
