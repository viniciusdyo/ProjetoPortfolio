using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProjetoPortfolio.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                option.LoginPath = "/Autenticacao/Registrar";
                option.LogoutPath= "/Autenticacao/Logout";
                option.AccessDeniedPath= "/Autenticacao/AcessoNegado";
                option.Cookie.SameSite = SameSiteMode.Strict;
                option.Cookie.HttpOnly= true;
                option.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                option.Cookie.Name = "VinidyJwtApp";
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "Admin",
                  pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
                );

                

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            

            app.Run();
        }
    }
}