using FinancialSystem.Application.Configuration;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Interfaces;
using FinancialSystem.Domain.Operations;
using FinancialSystem.Infrastructure.Data;
using FinancialSystem.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace FinancialSystem.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File("logs/financial-system-logs.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        
        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        
        builder.Services.AddScoped<IBankService, BankService>();
        builder.Services.AddScoped<IBankRepository, BankRepository>();
        
        builder.Services.AddScoped<IEnterpriseService, EnterpriseService>();
        builder.Services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
        
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IUserAccountService, UserAccountService>();
        builder.Services.AddScoped<IEnterpriseAccountService, EnterpriseAccountService>();
        
        builder.Services.AddScoped<ILoanService, LoanService>();
        builder.Services.AddScoped<ILoanRepository, LoanRepository>();
        
        builder.Services.AddScoped<IInstallmentService, InstallmentService>();
        builder.Services.AddScoped<IInstallmentRepository, InstallmentRepository>();
        
        builder.Services.AddScoped<ITransferService, TransferService>();
        builder.Services.AddScoped<ITransferRepository, TransferRepository>();
        
        builder.Services.AddScoped<ISalaryProjectRepository, SalaryProjectRepository>();
        builder.Services.AddScoped<ISalaryProjectService, SalaryProjectService>();
        
        builder.Services.AddScoped<IUserEnterpriseRepository, UserEnterpriseRepository>();

        builder.Services.AddScoped<ISalaryProjectEmployeeRepository, SalaryProjectEmployeeRepository>();

        builder.Services.AddScoped<OperationService>();
        builder.Services.AddScoped<IOperationLogRepository, OperationLogRepository>();

        builder.Services.AddScoped<IJwtService, JwtService>();
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