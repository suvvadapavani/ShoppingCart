using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingCart.Data;
using ShoppingCart.Services;
using ShoppingCart.Services.ServiceInterfaces;

namespace ShoppingCart
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            //register identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Register Dependency Injection
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IPaymentService, StripeService>();

            //Registering session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(30);//session time out
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;

            }
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            async Task SeedRoles(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Admin", "Manager", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }


            }
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRoles(services);
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseStaticFiles();   // <-- important
            app.UseRouting();
            app.UseAuthorization();// check if user is allowed
            app.UseSession(); //it should be before app.useendpoint or mapcontollers


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
