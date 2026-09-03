using Microsoft.EntityFrameworkCore;
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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();

            builder.Services.AddScoped<IBomService, BomService>();

            builder.Services.AddScoped<IBomRepository, BomRepository>();

            builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();

            builder.Services.AddScoped<IHistoryService, HistoryService>();

            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            
            builder.Services.AddScoped<IProductService, ProductService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
