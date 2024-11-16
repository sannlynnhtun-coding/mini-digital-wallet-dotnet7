using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniDigitalWallet.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    public virtual DbSet<TblWalletUser> TblWalletUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Tbl_Tran__55433A6B1846FC5B");

            entity.ToTable("Tbl_Transactions");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.SenderUser).WithMany(p => p.TblTransactions)
                .HasForeignKey(d => d.SenderUserId)
                .HasConstraintName("FK__Tbl_Trans__Sende__3B75D760");
        });

        modelBuilder.Entity<TblWalletUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Tbl_Wall__1788CC4C283A879F");

            entity.ToTable("Tbl_WalletUsers");

            entity.HasIndex(e => e.MobileNumber, "UQ__Tbl_Wall__250375B1AAB15C2D").IsUnique();

            entity.Property(e => e.Balance)
                .HasDefaultValueSql("((0.00))")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('active')");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
