using MedLinkDashboard.Data;
using MedLinkDashboard.IRepository;
using MedLinkDashboard.IService;
using MedLinkDashboard.Models;
using MedLinkDashboard.Repository;
using MedLinkDashboard.Service;
using MedLinkDashboard.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedLinkDashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Register Medlink DBContext
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            //Register Identity
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //require confirmed email to sign in
                options.SignIn.RequireConfirmedEmail = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true; // new users can be locked
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

            //Configure Identity Tokens
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                // Set token lifetime to 4
                options.TokenLifespan = TimeSpan.FromHours(4);
            });

            //Configure Application Cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "MedLinkAuth";// Custom cookie name
                options.Cookie.HttpOnly = true;// Prevent JS access (default true)

                options.SlidingExpiration = true;// Extend expiration if user active
                options.ExpireTimeSpan = TimeSpan.FromHours(2); // Cookie lifetime

                options.LoginPath = "/auth/Login";// Redirect here if not authenticated
                options.AccessDeniedPath = "/auth/AccessDenied"; // If forbidden
            });


            //Add Session
            builder.Services.AddSession();

            //Custom Services
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ISpecialityRepository, SpecialityRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.Configure<EmailSettings>
                (builder.Configuration.GetSection("SendGrid"));


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

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
