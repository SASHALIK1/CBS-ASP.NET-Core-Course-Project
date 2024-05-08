using CBS_ASP.NET_Core_Course_Project.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CBS_ASP.NET_Core_Course_Project
{

    public class Program
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/signin";
    });
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<ExchangeRateService>();
            //< IMyDependency, MyDependency > ();

            var app = builder.Build();

            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
