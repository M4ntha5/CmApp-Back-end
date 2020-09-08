using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmApp.BusinessLogic.Repositories;
using CmApp.BusinessLogic.Services;
using CmApp.Contracts;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CmApp.WebApi
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
            services.AddCors();

            services.AddScoped<ICarRepository, CarRepository>()
                    .AddScoped<IEmailRepository, EmailRepository>()
                    .AddScoped<IFileRepository, FileRepository>()
                    .AddScoped<IRepairRepository, RepairRepository>()
                    .AddScoped<IShippingRepository, ShippingRepository>()
                    .AddScoped<ISummaryRepository, SummaryRepository>()
                    .AddScoped<ITrackingRepository, TrackingRepository>()
                    .AddScoped<IUserRepository, UserRepository>()

                    .AddScoped<IAuthService, AuthService>()
                    .AddScoped<ICarService, CarService>()
                    .AddScoped<IExternalAPIService, ExternalAPIService>()
                    .AddScoped<IRepairService, RepairService>()
                    .AddScoped<IScraperService, ScraperService>()
                    .AddScoped<IShippingService, ShippingService>()
                    .AddScoped<ISummaryService, SummaryService>()
                    .AddScoped<ITrackingService, TrackingService>()

                    .AddScoped<DatabaseContext>()

                    ;


            //Settings.ProjectId = Guid.Parse(Environment.GetEnvironmentVariable("ProjectId"));
            //Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            //Settings.CaptchaApiKey = Environment.GetEnvironmentVariable("CaptchaApiKey");
            //Settings.DefaultImageUrl = Environment.GetEnvironmentVariable("DefaultImageUrl");
            //Settings.WelcomeEmailTemplateId = Environment.GetEnvironmentVariable("WelcomeEmailTemplateId");
            //Settings.PasswordResetEmailTemplateId = Environment.GetEnvironmentVariable("PasswordResetEmailTemplateId");
            //Settings.EmailConfirmationTemplateId = Environment.GetEnvironmentVariable("EmailConfirmationTemplateId");
            //Settings.SendGridApiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            //Settings.SenderEmailAddress = Environment.GetEnvironmentVariable("SenderEmailAddress");
            //Settings.SenderEmailAddressName = Environment.GetEnvironmentVariable("SenderEmailAddressName");
            //Settings.UserKey = Environment.GetEnvironmentVariable("TestUser");
            //Settings.AdminKey = Environment.GetEnvironmentVariable("TestUser");
            //Settings.Image4IoApiKey = Environment.GetEnvironmentVariable("Image4IoApiKey");
            //Settings.Image4IoSecret = Environment.GetEnvironmentVariable("Image4IoSecret");

            var symmetricSecurityKeyDefault = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.UserKey));
            var symmetricSecurityKeyAdmin = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.AdminKey));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("user", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "shrouded-ocean-70036.herokuapp.com",
                        ValidAudience = "readers",
                        IssuerSigningKey = symmetricSecurityKeyDefault
                    };
                })
                .AddJwtBearer("admin", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "shrouded-ocean-70036.herokuapp.com",
                        ValidAudience = "readers",
                        IssuerSigningKey = symmetricSecurityKeyAdmin
                    };

                });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    "user");
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    "admin");
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });


            services.AddMvcCore().AddNewtonsoftJson();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
