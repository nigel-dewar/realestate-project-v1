using System;
using GreenPipes;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications.Email;
using Notifications.Email.Models;
using Notifications.Email.Models.Interfaces;
using Notifications.Service.Messages.Consumers;
using Notifications.Service.Models;
using Notifications.Service.Services;
using Notifications.SMS;


namespace Notifications.Service
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
            services.AddTransient<ISmsService, SmsService>();
            
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            // services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IEmailService, EmailService>();
            
            #endregion

            #region MessageBus

            services.AddMassTransit(
                c =>
                {
                    c.AddConsumer<EmailConfirmationCommandConsumer>();
                    c.AddConsumer<EmailWelcomeCommandConsumer>();
                    c.AddConsumer<SmsVerifcationCommandConsumer>();
                    c.AddConsumer<EmailGenericCommandConsumer>();
                    
                } );

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(Configuration["EventBus:HostName"], "/", h =>
                {
                    h.Username(Configuration["EventBus:UserName"]);
                    h.Password(Configuration["EventBus:Password"]);
                });
                
                cfg.ReceiveEndpoint(EventBusConstants.EmailConfirmationCommandQueue, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                    e.Consumer<EmailConfirmationCommandConsumer>(provider);

                });
                cfg.ReceiveEndpoint(EventBusConstants.EmailWelcomeCommandQueue, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                    e.Consumer<EmailWelcomeCommandConsumer>(provider);

                });
                
                cfg.ReceiveEndpoint(EventBusConstants.SmsVerificationCommandQueue, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                    e.Consumer<SmsVerifcationCommandConsumer>(provider);

                });
                
                cfg.ReceiveEndpoint(EventBusConstants.EmailGenericCommand, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                    e.Consumer<EmailGenericCommandConsumer>(provider);

                });

                cfg.ConfigureEndpoints(provider);
                
            }));
            services.AddSingleton<IHostedService, BusService>();

            #endregion
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("AllowAllOriginsHeadersAndMethods");
            
            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
