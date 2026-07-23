using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Services;
using ShoppingCart.Services.ServiceInterfaces;

namespace ShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //    var builder = WebApplication.CreateBuilder(args);

            //    // Add services to the container.
            //    builder.Services.AddControllersWithViews();

            //    //register Dependency Injection
            //    builder.Services.AddScoped<INotificationService, NotificationService>();
            //    builder.Services.AddScoped<IPaymentService, StripeService>();

            //    var app = builder.Build();

            //    // Configure the HTTP request pipeline.
            //    if (!app.Environment.IsDevelopment())
            //    {
            //        app.UseExceptionHandler("/Home/Error");
            //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //        app.UseHsts();
            //    }

            //    app.UseHttpsRedirection();
            //    app.UseStaticFiles();
            //    app.UseRouting();

            //    app.UseAuthorization();

            //    app.MapStaticAssets();
            //    app.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}")
            //        .WithStaticAssets();

            //    app.Run();
            //}
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });
           

            //Register Dbcontect
            builder.Services.AddDbContext<ApplicationDbContext>
                (
               options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"))
                
                );

            // Register Dependency Injection
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IPaymentService, StripeService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseStaticFiles();   // <-- important
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
