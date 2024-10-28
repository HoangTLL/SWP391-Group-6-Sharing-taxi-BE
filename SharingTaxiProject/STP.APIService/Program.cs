﻿using Microsoft.EntityFrameworkCore;
using STP.Repository;
using STP.Repository.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace STP.APIService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            // Cấu hình CORS cho phép tất cả
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin() // Cho phép tất cả các nguồn
                           .AllowAnyHeader() // Cho phép tất cả các header
                           .AllowAnyMethod(); // Cho phép tất cả các phương thức (GET, POST, v.v.)
                           
                });
            });


            // Cấu hình xác thực
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })

            .AddCookie()
            // cấu hình Google Authentication
            .AddGoogle(options =>
{
             options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
             options.CallbackPath = "/api/GoogleLogin/google-callback";
            options.Scope.Add("profile");
            options.Scope.Add("email");

    // Cập nhật xử lý redirect
    options.Events = new OAuthEvents
    {
        OnRedirectToAuthorizationEndpoint = context =>
        {
            string redirectUri;
            var originUrl = context.Request.Headers["Origin"].FirstOrDefault() ??
                          $"{context.Request.Scheme}://{context.Request.Host}";

            if (originUrl.Contains("localhost"))
            {
                redirectUri = "http://localhost:5174/api/GoogleLogin/google-callback";
            }
            else
            {
                redirectUri = "http://sharetaxi.somee.com/api/GoogleLogin/google-callback";
            }

            var authEndpoint = context.RedirectUri.Split('?')[0];
            var queryString = context.RedirectUri.Split('?')[1];
            var newRedirectUri = $"{authEndpoint}?{queryString.Replace(
                Uri.EscapeDataString(context.RedirectUri),
                Uri.EscapeDataString(redirectUri)
            )}";

            context.Response.Redirect(newRedirectUri);
            return Task.CompletedTask;
        }
    };
})
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Đăng ký DbContext
            builder.Services.AddDbContext<ShareTaxiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Cấu hình logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.Services.AddEndpointsApiExplorer();

            // Cấu hình Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "STP API", Version = "v1" });

                c.AddSecurityDefinition("google_auth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth"),
                            TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"openid", "OpenID"},
                                {"profile", "Profile"},
                                {"email", "Email"}
                            }
                        }
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Đăng ký UnitOfWork
            builder.Services.AddScoped<UnitOfWork>();
            // Đăng ký WalletRepository
            builder.Services.AddScoped<WalletRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "STP API V1");
                    c.OAuthClientId(builder.Configuration["Authentication:Google:ClientId"]);
                    c.OAuthAppName("STP Google Auth");
                });
            }

            app.UseRouting();
            app.UseCors("AllowAll"); // Bật CORS với cấu hình cho phép tất cả các nguồn
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}