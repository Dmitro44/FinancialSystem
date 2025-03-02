using FinancialSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<Enterprise> Enterprises  { get; set; }
    public DbSet<Installment> Installments { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<SalaryProject> SalaryProjects { get; set; }
    public DbSet<UserBankRole> UserBankRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey(a => a.OwnerId);
        
        modelBuilder.Entity<Account>()
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

        modelBuilder.Entity<Enterprise>()
            .HasOne(e => e.Bank)
            .WithMany(b => b.Enterprises)
            .HasForeignKey("BankId");
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.IdentificationNumber)
            .IsUnique();
    }
}