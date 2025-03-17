using FinancialSystem.Application.Configuration;
using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Interfaces;
using FinancialSystem.Infrastructure.Data;
using FinancialSystem.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        
        builder.Services.AddScoped<BankService>();
        builder.Services.AddScoped<IBankRepository, BankRepository>();
        
        builder.Services.AddScoped<AccountService>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        
        builder.Services.AddScoped<LoanService>();
        builder.Services.AddScoped<ILoanRepository, LoanRepository>();
        
        builder.Services.AddScoped<InstallmentService>();
        builder.Services.AddScoped<IInstallmentRepository, InstallmentRepository>();

        builder.Services.AddScoped<JwtService>();
        builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
        builder.Services.AddAuth(builder.Configuration);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _ = context.Banks.FirstOrDefault(); // Simple request for warmup
        }

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}