using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Repositories;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PortfolioDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();

            var app = builder.Build();

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