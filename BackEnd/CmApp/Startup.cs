using CmApp.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CmApp
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

            Settings.ProjectId = Guid.Parse("9b932d71-5bb4-4579-9d69-c61e6dbca245");
            Settings.ApiKey = "euHlhRE-TGmz4Y5gtmNeoFm1L5e_Xkun";
            Settings.CaptchaApiKey = "1d53bbbfbc8e66ffbfef9b172fa5d183";
            Settings.DefaultImage = "b9daf2e7-e48f-4d77-89c8-b872013ff9a1";
            Settings.DefaultImageUrl = "https://cdn.image4.io/cmapp//272f3362-1457-4aa9-89c4-491c5994149e.jpg";
            Settings.WelcomeEmailTemplateId = "d-ad30b9067e184a9e894774a0d2273fd0";
            Settings.PasswordResetEmailTemplateId = "d-b08639c3bb1c4804ba69a2a09d3b5123";
            Settings.EmailConfirmationTemplateId = "d-99633e1621ec45a6bc60c31cc0491f39";
            Settings.SendGridApiKey = "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";
            Settings.SenderEmailAddress = "mantas.daunoravicius@ktu.edu";
            Settings.SenderEmailAddressName = "CmApp";
            Settings.UserKey = "this_is_user_key";
            Settings.AdminKey = "this_is_admin_key";
            Settings.Image4IoApiKey = "VCpoVQjWHblJ0/5/nRaxoQ==";
            Settings.Image4IoSecret = "GZ3/NKR+237ZMiY4fmPfcotHPf7DYrizFef2aJ6JwqY=";
            Settings.ExchangeApiKey = "4857d5067ad928fd909e5350d9004c1a";

            /*Settings.ProjectId = Guid.Parse(Environment.GetEnvironmentVariable("ProjectId"));
            Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            Settings.CaptchaApiKey = Environment.GetEnvironmentVariable("CaptchaApiKey");
            Settings.DefaultImage = Environment.GetEnvironmentVariable("DefaultImage");
            Settings.DefaultImageUrl = Environment.GetEnvironmentVariable("DefaultImageUrl");
            Settings.WelcomeEmailTemplateId = Environment.GetEnvironmentVariable("WelcomeEmailTemplateId");
            Settings.PasswordResetEmailTemplateId = Environment.GetEnvironmentVariable("PasswordResetEmailTemplateId");
            Settings.EmailConfirmationTemplateId = Environment.GetEnvironmentVariable("EmailConfirmationTemplateId");
            Settings.SendGridApiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            Settings.SenderEmailAddress = Environment.GetEnvironmentVariable("SenderEmailAddress");
            Settings.SenderEmailAddressName = Environment.GetEnvironmentVariable("SenderEmailAddressName");
            Settings.UserKey = Environment.GetEnvironmentVariable("TestUser");
            Settings.AdminKey = Environment.GetEnvironmentVariable("TestUser");
            Settings.Image4IoApiKey = Environment.GetEnvironmentVariable("Image4IoApiKey");
            Settings.Image4IoSecret = Environment.GetEnvironmentVariable("Image4IoSecret");
            Settings.ExchangeApiKey = Environment.GetEnvironmentVariable("ExchangeApiKey");
            */

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
