using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Repository.Entity;

namespace Repository
{
    public partial class ExchangeBotDbContext : DbContext
    {
        public ExchangeBotDbContext()
        {
        }

        public ExchangeBotDbContext(DbContextOptions<ExchangeBotDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BotHistory> BotHistories { get; set; }
        public virtual DbSet<ChatDetail> ChatDetails { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public DbSet<BankLocation> Locations { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserStatus> UserStatuses { get; set; }
        public virtual DbSet<UsersActivityHistory> UsersActivityHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-DP52C93\\SQLEXPRESS;Database=ExchangeBotDb;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BankUrl)
                    .HasMaxLength(500)
                    .HasColumnName("BankURL");
            });

            modelBuilder.Entity<BotHistory>(entity =>
            {
                entity.ToTable("BotHistory");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<ChatDetail>(entity =>
            {
                entity.HasIndex(e => e.Id, "IX_ChatDetails_Id");

                entity.HasIndex(e => e.UserActivityHistoryId, "IX_ChatDetails_UserActivityHistoryId");

                entity.Property(e => e.MessageExternalId).HasDefaultValueSql("(CONVERT([bigint],(0)))");

                entity.HasOne(d => d.UserActivityHistory)
                    .WithMany(p => p.ChatDetails)
                    .HasForeignKey(d => d.UserActivityHistoryId);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Currencies_1");

                entity.Property(e => e.Code).HasMaxLength(3);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(e => e.BankId, "IX_Locations_BankId");

                entity.HasIndex(e => e.Id, "IX_Locations_Id");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.BankId);
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.HasIndex(e => e.BankId, "IX_Rates_BankId");

                entity.HasIndex(e => e.FromCurrency, "IX_Rates_FromCurrency");

                entity.HasIndex(e => e.ToCurrency, "IX_Rates_ToCurrency");

                entity.Property(e => e.BuyValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FromCurrency)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.SellValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ToCurrency)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rates_Banks");

                entity.HasOne(d => d.FromCurrencyNavigation)
                    .WithMany(p => p.RateFromCurrencyNavigations)
                    .HasForeignKey(d => d.FromCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rates_Currencies");

                entity.HasOne(d => d.ToCurrencyNavigation)
                    .WithMany(p => p.RateToCurrencyNavigations)
                    .HasForeignKey(d => d.ToCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rates_Currencies1");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.ToTable("UserStatus");
            });

            modelBuilder.Entity<UsersActivityHistory>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_UsersActivityHistories_RoleId");

                entity.HasIndex(e => e.StatusId, "IX_UsersActivityHistories_StatusId");

                entity.Property(e => e.RoleId).HasDefaultValueSql("(CONVERT([smallint],(2)))");

                entity.Property(e => e.StatusId).HasDefaultValueSql("(CONVERT([smallint],(1)))");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UsersActivityHistories)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.UsersActivityHistories)
                    .HasForeignKey(d => d.StatusId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
