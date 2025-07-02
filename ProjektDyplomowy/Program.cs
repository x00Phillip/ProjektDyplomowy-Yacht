using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.Data;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using ProjektDyplomowy.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddTransient<IEmailSender, GmailEmailSender>();
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) //zmieniæ do testowania maila z linkiem
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];
    });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Manager", "Partner", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string admin_email = "admin@admin.com";
            string admin_password = "1qaz!QAZ";
            string manager1_email = "filip@manager.com";
            string manager1_password = "1qaz!QAZ";
            string manager2_email = "patryk@manager.com";
            string manager2_password = "1qaz!QAZ";
            string manager3_email = "mateusz@manager.com";
            string manager3_password = "1qaz!QAZ";

            if (await userManager.FindByEmailAsync(admin_email) == null)
            {
                var admin = new IdentityUser();
                admin.Email = admin_email;
                admin.UserName = admin_email;

                await userManager.CreateAsync(admin, admin_password);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            if ((await userManager.FindByEmailAsync(manager1_email)) == null &&
                (await userManager.FindByEmailAsync(manager2_email)) == null &&
                (await userManager.FindByEmailAsync(manager3_email)) == null)
            {
                var manager1 = new IdentityUser();
                manager1.Email = manager1_email;
                manager1.UserName = manager1_email;


                var manager2 = new IdentityUser();
                manager2.Email = manager2_email;
                manager2.UserName = manager2_email;

                var manager3 = new IdentityUser();
                manager3.Email = manager3_email;
                manager3.UserName = manager3_email;


                await userManager.CreateAsync(manager1, manager1_password);
                await userManager.AddToRoleAsync(manager1, "Manager");
                await userManager.CreateAsync(manager2, manager2_password);
                await userManager.AddToRoleAsync(manager2, "Manager");
                await userManager.CreateAsync(manager3, manager3_password);
                await userManager.AddToRoleAsync(manager3, "Manager");
            }
        }
        app.Run();
    }
}

