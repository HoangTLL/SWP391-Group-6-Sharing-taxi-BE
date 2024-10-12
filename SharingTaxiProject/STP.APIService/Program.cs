using Microsoft.EntityFrameworkCore; // Thêm namespace cho EntityFramework
using STP.Repository;
using STP.Repository.Models; // Namespace chứa ShareTaxiContext
using System.Text.Json.Serialization;

namespace STP.APIService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            // Cấu hình CORS cho phép tất cả các frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()   // Cho phép tất cả các frontend truy cập
                               .AllowAnyHeader()   // Cho phép tất cả các header
                               .AllowAnyMethod();  // Cho phép tất cả các phương thức HTTP
                    });
            });

            // Đăng ký DbContext (ShareTaxiContext) với DI container
            builder.Services.AddDbContext<ShareTaxiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Đăng ký các dịch vụ khác
            builder.Services.AddScoped<UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Sử dụng chính sách CORS cho phép tất cả nguồn
            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();

            // ASP.NET Core sẽ tự động tìm và ánh xạ tất cả các controller
            app.MapControllers();

            app.Run();
        }
    }
}
