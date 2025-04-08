using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FinancialSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<UserAccount?> UserAccounts { get; set; }
    public DbSet<EnterpriseAccount> EnterpriseAccounts { get; set; }
    public DbSet<Enterprise> Enterprises  { get; set; }
    public DbSet<Installment> Installments { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<SalaryProject> SalaryProjects { get; set; }
    public DbSet<UserBankRole> UserBankRoles { get; set; }
    public DbSet<UserEnterprise> UserEnterprises { get; set; }
    public DbSet<SalaryProjectEmployee> SalaryProjectEmployees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .ToTable("Accounts")
            .HasDiscriminator(a => a.Discriminator)
            .HasValue<UserAccount>(AccountDiscriminator.User)
            .HasValue<EnterpriseAccount>(AccountDiscriminator.Enterprise);
        
        modelBuilder.Entity<EnterpriseAccount>()
            .HasOne(ea => ea.Enterprise)
            .WithMany()
            .HasForeignKey(ea => ea.EnterpriseId);
        
        modelBuilder.Entity<UserAccount>()
            .HasOne(ua => ua.Owner)
            .WithMany()
            .HasForeignKey(ua => ua.OwnerId);
        
        modelBuilder.Entity<UserAccount>()
            .HasOne(ua => ua.EmployerEnterprise)
            .WithMany(e => e.EmployeeAccounts)
            .HasForeignKey(ua => ua.EmployerEnterpriseId);
        
        modelBuilder.Entity<UserAccount>()
            .HasOne(a => a.Bank)
            .WithMany(b => b.UserAccounts)
            .HasForeignKey(a => a.BankId);

        modelBuilder.Entity<UserBankRole>()
            .HasOne(ubr => ubr.User)
            .WithMany(u => u.UserBankRoles)
            .HasForeignKey(ubr => ubr.UserId);
        
        modelBuilder.Entity<UserBankRole>()
            .HasOne(ubr => ubr.Bank)
            .WithMany(b => b.UserBankRoles)
            .HasForeignKey(ubr => ubr.BankId);
        
        modelBuilder.Entity<UserEnterprise>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.UserEnterprises)
            .HasForeignKey(ue => ue.UserId);

        modelBuilder.Entity<UserEnterprise>()
            .HasOne(ue => ue.Enterprise)
            .WithMany()
            .HasForeignKey(ue => ue.EnterpriseId);

        modelBuilder.Entity<Enterprise>()
            .HasOne(e => e.Bank)
            .WithMany(b => b.Enterprises)
            .HasForeignKey("BankId");
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.IdentificationNumber)
            .IsUnique();

        modelBuilder.Entity<SalaryProjectEmployee>()
            .HasOne(spe => spe.UserAccount)
            .WithMany()
            .HasForeignKey(spe => spe.UserAccountId);
        
        // Настройка Transfer с поддержкой полиморфных связей
        modelBuilder.Entity<Transfer>()
            .ToTable("Transfers")
            .HasOne(t => t.Sender)
            .WithMany()
            .HasForeignKey(t => t.SenderId)
            .OnDelete(DeleteBehavior.Restrict); // Предотвращаем каскадное удаление
    
        modelBuilder.Entity<Transfer>()
            .HasOne(t => t.Receiver)
            .WithMany()
            .HasForeignKey(t => t.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict); // Предотвращаем каскадное удаление
    }
}