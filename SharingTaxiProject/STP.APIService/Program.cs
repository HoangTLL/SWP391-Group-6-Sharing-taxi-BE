using Microsoft.EntityFrameworkCore;
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

            // Thêm dịch vụ Controllers và cấu hình JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            // Cấu hình CORS cho phép từ localhost:5173
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()       // Cho phép tất cả các nguồn gốc
                           .AllowAnyHeader()       // Cho phép tất cả các header
                           .AllowAnyMethod();       // Cho phép tất cả các phương thức (GET, POST, v.v.)
                           
                });
            });

            // Cấu hình xác thực (Google OAuth và JWT)
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    options.SaveTokens = true; // Lưu token để sử dụng sau này
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

            // Đăng ký Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "STP API", Version = "v1" });

                // Cấu hình OAuth cho Swagger
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
                                { "openid", "OpenID" },
                                { "profile", "Profile" },
                                { "email", "Email" }
                            }
                        }
                    }
                });

                // Cấu hình JWT cho Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Cấu hình yêu cầu bảo mật cho các endpoint
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

            // Đăng ký các lớp xử lý dữ liệu
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<WalletRepository>();

            var app = builder.Build();

            // Cấu hình HTTP request pipeline
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

            // Middleware
            app.UseHttpsRedirection();
            app.UseCors("AllowAll"); 

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}