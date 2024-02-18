using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;


namespace SportsStore
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        private IConfiguration Configuration { get; set; }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<StoreDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:SportsStoreConnection"]);
            });
            services.AddScoped<IStoreRepository, EFStoreRepository>();//хранилище
            services.AddRazorPages();
            services.AddDistributedMemoryCache(); //настраивает хранилище данных
            services.AddSession();//регистрирует службы используемые для доступа к данным сеанса
            services.AddScoped<Cart>(sp=>SessionCart.GetCart(sp)); //Разные корзины у пользователей
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//Для работы с сессиями
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession(); //позволяет системе сеансов автоматически ассоциировать запросы с сеансами когда они поступают от клиента
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", action = "Index" });
                endpoints.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("pagination","Products/Page{productPage}", new {Controller ="Home", action="Index", productPage=1});
                
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
            SeedData.EnsurePopulated(app);
        }
    }
}
