using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using GreenPipes;
using Manage.API.Interfaces;
using Manage.API.Messages.Consumers;
using Manage.API.Middleware;
using Manage.API.Services;
using Manage.API.Services.v1;
using Manage.API.Settings;
using Manage.Repository.Data;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using MessageBus.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Manage.API
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
            services.AddDbContext<ManageDataContext>(options =>
            {
                //options.UseLazyLoadingProxies();
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            // services.AddDbContextPool<ManageDataContext>(options => options
            //     // replace with your connection string
            //     .UseMySql(Configuration.GetConnectionString("DefaultConnection"), mySqlOptions => mySqlOptions
            //         // replace with your Server Version and Type
            //         .ServerVersion(new Version(8, 0, 18), ServerType.MySql)
            //     ));
            
            

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
            #endregion
            
            services.AddAutoMapper(typeof(PropertiesService));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            // services.AddMvc(opt =>
            // {
            //     var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //     opt.Filters.Add(new AuthorizeFilter(policy));
            // }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            // Register Core Estatetify Services
            
            services.AddScoped<IPropertiesService, PropertiesService>();
            services.AddScoped<IPostCodesRepository, PostCodesRepository>();
            services.AddScoped<IListingService, ListingService>();
            services.AddScoped<IMailService, MailService>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IUserAccessor, UserAccessor>();
            services.AddHttpContextAccessor();
            
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
            services.AddScoped<IImageService, CloudinaryService>();
            

            #region Auth
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JwtKey"]));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.ClaimsIssuer = Configuration["Authentication:Issuer"];
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidAudience = Configuration["Authentication:Audience"],
                    ValidateIssuer = false,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
            
            #endregion
            
            #region ServiceBus
            services.AddMassTransit(c =>
                {
                    c.AddConsumer<GetManageListingsCommandConsumer>();
                }
            );
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(Configuration["EventBus:HostName"], "/", h =>
                {
                    h.Username(Configuration["EventBus:UserName"]);
                    h.Password(Configuration["EventBus:Password"]);
                });
                
                cfg.ReceiveEndpoint(EventBusConstants.GetLookupsFromManageApiCommandQueue, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                    e.Consumer<GetManageListingsCommandConsumer>(provider);

                });

                cfg.ConfigureEndpoints(provider);
                
            }));
            services.AddSingleton<IHostedService, BusService>();

            #endregion
        }
        
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }
            
            
            
            app.UseCors("AllowAllOriginsHeadersAndMethods");
            
            app.UseAuthentication();

            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}