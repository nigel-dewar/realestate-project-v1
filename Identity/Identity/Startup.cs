using System;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Identity.API.Data;
using Identity.API.Entities;
using Identity.API.Middleware;
using Identity.API.Services;
using MassTransit;
using MessageBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Identity.API
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
            
            services.AddDbContext<DataContext>(options =>
            {
                //options.UseLazyLoadingProxies();
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOriginsHeadersAndMethods",
                    policy => policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials());
            });
            
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            // Add AspNet Identity
            var builder = services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            });
                
            var identityBuilder = new IdentityBuilder(builder.UserType,  builder.Services);
            identityBuilder.AddSignInManager<SignInManager<AppUser>>();
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddDefaultTokenProviders();
            identityBuilder.AddEntityFrameworkStores<DataContext>();

            // services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IMailService, SendGridMailService>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddHttpContextAccessor();
            // Get key from Local PC secrets
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
            
            #region ServiceBus
            services.AddMassTransit();
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(
                cfg =>
                {
                    var host = cfg.Host(Configuration["EventBus:HostName"], "/", h =>
                    {
                        h.Username(Configuration["EventBus:UserName"]);
                        h.Password(Configuration["EventBus:Password"]);
                    });
                    services.AddSingleton(x => x.GetRequiredService<IBusControl>());
                    services.AddSingleton<IHostedService, BusService>();

                }));

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }
            
            app.UseCors("AllowAllOriginsHeadersAndMethods");

            app.UseStaticFiles();
            
            app.UseAuthentication();

            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}