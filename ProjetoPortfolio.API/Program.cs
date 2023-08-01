using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Repositories;
using ProjetoPortfolio.API.Repositories.Interfaces;
using ProjetoPortfolio.API.Services;
using ProjetoPortfolio.API.Services.Interfaces;
using System;
using System.Text;

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

            var jwtAudience = builder.Configuration.GetSection("JwtConfigs:Key").Value;
            var jwtIssuer = builder.Configuration.GetSection("JwtConfigs:Issuer").Value;
            var jwtKey = builder.Configuration.GetSection("JwtConfigs:Audience").Value;

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

            builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
            builder.Services.AddScoped<IRegistroRepository, RegistroRepository>();
            builder.Services.AddTransient<ITokenService, TokenService>();


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<PortfolioDbContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}