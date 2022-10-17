using Microsoft.EntityFrameworkCore;
using ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStorage_WPF_Dapper.MVVM.Models.DBModels.Contexts
{
    public class EntityContext:DbContext
    {

        public DbSet<Product> Products { get; set; }

        public EntityContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(App.ConnStr);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Name
            modelBuilder.Entity<Product>()
                           .Property("Name")
                           .IsUnicode()
                           .HasMaxLength(50);
            modelBuilder.Entity<Product>().HasIndex("Name").IsUnique(); // Make Unique

            // Description
            modelBuilder.Entity<Product>()
                           .Property("Description")
                           .IsUnicode();

            // ImageSource
            modelBuilder.Entity<Product>()
                           .Property("ImageSource")
                           .IsUnicode();

            // OriginCountry
            modelBuilder.Entity<Product>()
               .Property("OriginCountry")
               .IsUnicode();

            // Price
            modelBuilder.Entity<Product>()
               .Property("Price")
               .HasColumnType("money");

            // Flag
            modelBuilder.Entity<Product>().Ignore("Flag");

        }

    }
}
