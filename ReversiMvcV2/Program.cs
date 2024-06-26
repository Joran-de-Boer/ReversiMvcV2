using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReversiMvcV2.DAL;
using ReversiMvcV2.Data;
using ReversiMvcV2.Hubs;
using ReversiMvcV2.Models;
using System.Drawing.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

connectionString = builder.Configuration.GetConnectionString("ReversiDb2");
builder.Services.AddDbContext<SpelerContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddSignalR();
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddCookiePolicy((options =>
{
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None;
    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
   //app.UseExceptionHandler("/Home/Error");
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<ReversiHub>("/ReversiHub");

app.Run();

namespace configure
{
    static class Configure
    {
        public static void configure(IApplicationBuilder app)
        {
            CreateRoles(app.ApplicationServices).Wait();
        }

        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Initializing custom roles
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            string[] roleNames = { "BEHEERDER", "MEDIATOR", "SPELER" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Check if the role exists, and create it if it does not
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }
    }
}