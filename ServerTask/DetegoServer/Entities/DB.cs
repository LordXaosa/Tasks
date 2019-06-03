using System;
using System.Linq;
using DetegoServer.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DetegoServer.Entities
{
    public partial class DB : DbContext
    {
        public DB()
        {
        }

        public DB(DbContextOptions<DB> options)
            : base(options)
        {
        }

        public virtual DbSet<StoreData> StoreData { get; set; }
        public virtual DbSet<Stores> Stores { get; set; }
        public virtual DbQuery<VStockInfo> VStockInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(string.Format(ConfigHelper.Instance.ConnectionStrings["DBConnection"], AppDomain.CurrentDomain.BaseDirectory));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreData>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.Property(e => e.StoreId).ValueGeneratedNever();

                entity.HasOne(d => d.Store)
                    .WithOne(p => p.StoreData)
                    .HasForeignKey<StoreData>(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_storedata_store");
            });

            modelBuilder.Entity<Stores>(entity =>
            {
                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.StoreEmail)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.StoreMgrEmail)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.StoreMgrFirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StoreMgrLastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasColumnType("ntext");
            });
            modelBuilder.Query<VStockInfo>().ToView("VStockInfo");
        }
    }
}
