using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {

        public static void Seed(RestaurantDbContext context)
        {
            if (context.Database.CanConnect())
            {
                var pendingMigrations = context.Database.GetPendingMigrations();

                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Restaurants.Any())
                {
                    var dishGenerator = new Faker<Dish>()
                        .RuleFor(d => d.Name, f => f.Commerce.ProductName())
                        .RuleFor(d => d.Description, f => f.Lorem.Sentence())
                        .RuleFor(d => d.Price, f => decimal.Parse(f.Commerce.Price(5, 50)));

                    var addressGenerator = new Faker<Address>()
                        .RuleFor(a => a.City, f => f.Address.City())
                        .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                        .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());

                    var restaurants = new Faker<Restaurant>()
                        .RuleFor(r => r.Name, f =>
                        {
                            var name = f.Company.CompanyName();
                            return name.Length > 25 ? name.Substring(0, 25) : name;
                        })
                        .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                        .RuleFor(r => r.Category, f => f.PickRandom(new[] { "Italian", "Chinese", "Indian", "Mexican", "American" }))
                        .RuleFor(r => r.HasDelivery, f => f.Random.Bool())
                        .RuleFor(r => r.ContactEmail, f => f.Internet.Email())
                        .RuleFor(r => r.ContactNumber, f => f.Phone.PhoneNumber())
                        .RuleFor(r => r.Address, f => addressGenerator.Generate())
                        .RuleFor(r => r.Dishes, f => dishGenerator.Generate(f.Random.Int(2, 5)));

                    var restaurantList = restaurants.Generate(10);
                    context.Restaurants.AddRange(restaurantList);
                    context.SaveChanges();
                }

                if (!context.Roles.Any())
                {
                    var roles = new List<Role>
                    {
                        new Role { Name = "User" },
                        new Role { Name = "Manager" },
                        new Role { Name = "Admin" }
                    };

                    context.Roles.AddRange(roles);
                    context.SaveChanges();
                }
            }
        }
    }
}