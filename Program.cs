using Training.ProductApi1.Middleware;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Training.ProductApi1.Data;
using Training.ProductApi1.Repositories;
using Training.ProductApi1.Services;

namespace Training.ProductApi1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();

            builder.Host.UseNLog();

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));
            // Repository
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<IBomRepository, BomRepository>();
            builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();

            // Service
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IBomService, BomService>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
