using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.Data;


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

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

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
            string email = "admin@admin.com";
            string password = "1qaz!QAZ";
            string email2 = "filip@manager.com";
            string password2 = "1qaz!QAZ";
            string email3 = "patryk@manager.com";
            string password3 = "1qaz!QAZ";
            string email4 = "mateusz@manager.com";
            string password4 = "1qaz!QAZ";

            if(await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;
                await userManager.CreateAsync (user, password);
                var user2 = new IdentityUser();
                user2.UserName = email2;
                user2.Email = email2;
                await userManager.CreateAsync(user2, password2);
                var user3 = new IdentityUser();
                user3.UserName = email3;
                user3.Email = email3;
                await userManager.CreateAsync(user3, password3);
                var user4 = new IdentityUser();
                user4.UserName = email4;
                user4.Email = email4;
                await userManager.CreateAsync(user4, password4);

                userManager.AddToRoleAsync(user, "Admin");
                userManager.AddToRoleAsync(user2, "Manager");
                userManager.AddToRoleAsync(user3, "Manager");
                userManager.AddToRoleAsync(user4, "Manager");
            }
           
        }
        app.Run();
    }
}

