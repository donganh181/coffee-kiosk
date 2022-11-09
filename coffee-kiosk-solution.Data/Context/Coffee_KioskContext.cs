using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using coffee_kiosk_solution.Data.Models;

#nullable disable

namespace coffee_kiosk_solution.Data.Context
{
    public partial class Coffee_KioskContext : DbContext
    {
        public Coffee_KioskContext()
        {
        }

        public Coffee_KioskContext(DbContextOptions<Coffee_KioskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAccount> TblAccounts { get; set; }
        public virtual DbSet<TblArea> TblAreas { get; set; }
        public virtual DbSet<TblCampaign> TblCampaigns { get; set; }
        public virtual DbSet<TblCategory> TblCategories { get; set; }
        public virtual DbSet<TblDiscount> TblDiscounts { get; set; }
        public virtual DbSet<TblOrder> TblOrders { get; set; }
        public virtual DbSet<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblProductImage> TblProductImages { get; set; }
        public virtual DbSet<TblRole> TblRoles { get; set; }
        public virtual DbSet<TblShop> TblShops { get; set; }
        public virtual DbSet<TblSupply> TblSupplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblAccount>(entity =>
            {
                entity.ToTable("tblAccount");

                entity.HasIndex(e => e.Username, "IX_tblAccount")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.InverseCreator)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_tblAccount_tblAccount");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccount_tblRole");
            });

            modelBuilder.Entity<TblArea>(entity =>
            {
                entity.ToTable("tblArea");

                entity.HasIndex(e => e.AreaName, "IX_tblArea")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblCampaign>(entity =>
            {
                entity.ToTable("tblCampaign");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ExpiredDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StartingDate).HasColumnType("date");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.TblCampaigns)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCampaign_tblArea");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.HasIndex(e => e.Name, "IX_tblCategory")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TblDiscount>(entity =>
            {
                entity.ToTable("tblDiscount");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.TblDiscounts)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDiscount_tblCampaign");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.ToTable("tblOrder");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.No).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK_tblOrder_tblDiscount");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrder_tblShop");
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.ToTable("tblOrderDetail");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrderDetail_tblOrder");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOrderDetail_tblProduct");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("tblProduct");

                entity.HasIndex(e => e.Name, "IX_tblProduct")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduct_tblCategory");
            });

            modelBuilder.Entity<TblProductImage>(entity =>
            {
                entity.ToTable("tblProductImage");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductImage_tblProduct");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tblRole");

                entity.HasIndex(e => e.RoleName, "IX_tblRole")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblShop>(entity =>
            {
                entity.ToTable("tblShop");

                entity.HasIndex(e => e.Name, "IX_tblShop")
                    .IsUnique();

                entity.HasIndex(e => e.Address, "IX_tblShop_1")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblShops)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_tblShop_tblAccount");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.TblShops)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblShop_tblArea");
            });

            modelBuilder.Entity<TblSupply>(entity =>
            {
                entity.ToTable("tblSupply");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblSupplies)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSupply_tblProduct");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblSupplies)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSupply_tblShop");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
