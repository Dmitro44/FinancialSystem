using FinancialSystem.Domain.Entities;
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
    public DbSet<UserAccount> UserAccounts { get; set; }
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

        modelBuilder.Entity<UserAccount>()
            .HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey(a => a.OwnerId);
        
        modelBuilder.Entity<UserAccount>()
            .HasOne(a => a.Bank)
            .WithMany(b => b.Accounts)
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
    }
}