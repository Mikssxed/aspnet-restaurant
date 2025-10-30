using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>(r =>
            {
                r.HasOne(rest => rest.Address)
                 .WithOne(addr => addr.Restaurant)
                 .HasForeignKey<Restaurant>(rest => rest.AddressId);

                r.HasMany(rest => rest.Dishes)
                 .WithOne(dish => dish.Restaurant)
                 .HasForeignKey(dish => dish.RestaurantId);

                r.Property(rest => rest.Name).IsRequired().HasMaxLength(25);
            });

            modelBuilder.Entity<Dish>(d =>
            {
                d.Property(dish => dish.Name).IsRequired();
            });

            modelBuilder.Entity<Address>(a =>
            {
                a.Property(address => address.City).IsRequired().HasMaxLength(50);
                a.Property(address => address.Street).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<User>(u =>
            {
                u.Property(user => user.Email).IsRequired();
            });

            modelBuilder.Entity<Role>(r =>
            {
                r.Property(role => role.Name).IsRequired();
            });
        }
    }
}