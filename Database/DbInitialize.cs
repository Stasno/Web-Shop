using Database.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Database
{
    public class DbInitialize
    {
        public static void Initialize(ApplicationContext context, UserManager<User> userManager)
        {
            if (context.Users.Any() == false)
            {
                User user = new User
                {
                    Email = "test123@gmail.ru",
                    UserName = "test123@gmail.ru",
                    Firstname = "stas",
                    Secondname = "novikov"
                };
                _ = userManager.CreateAsync(user, "TEstPassword91433!!!!432").Result;
            }

            if (context.OrderStates.Any() == false)
            {
                var states = new OrderState[]
                {
                    new OrderState { Name = "Cancelled" },
                    new OrderState { Name = "Confirmed" },
                    new OrderState { Name = "Returned" },
                    new OrderState { Name = "Complited" },
                    new OrderState { Name = "Delivering" },
                };

                context.OrderStates.AddRange(states);

            }

            if (context.Products.Any() == false)
            {
                var tag = new Category[]
                {
                    new Category { Name = "Roses" },
                    new Category { Name = "Tulips" },
                    new Category { Name = "Irises" }
                };
                context.Categories.AddRange(tag);

                string[] categories = { "Roses", "Tulips", "Irises" };
                Product product;

                Random random = new();

                for (int i = 0; i < 100; i++)
                {
                    product = new();
                    int num = random.Next(10);

                    product.Categories.Add(tag[random.Next(3)]);

                    if (num > 7)
                    {
                        num = random.Next(3);
                        while (tag[num] == product.Categories[0])
                            num = random.Next(3);
                        product.Categories.Add(tag[num]);
                    }

                    product.Title = product.Categories[0].Name;
                    if (product.Categories.Count == 2)
                    {
                        product.Title += " and " + product.Categories[1].Name;
                    }

                    product.Title += " " + random.Next(1, 102);

                    product.Price += random.Next(200, 5000);

                    product.InStock += random.Next(21);

                    product.Description = "Description Description Description Description Description Description Description Description Description Description Description Description ";

                    context.Products.Add(product);

                }

                context.SaveChanges();

            }
        }
    }
}
