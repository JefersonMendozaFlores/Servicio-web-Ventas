using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Enums;
using ShoppingCart.Models;
using System;

namespace ShoppingCart.Infrastructure
{
    public class SeedData
    {
        public static void SeedDatabase(DataContext context)
        {

            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new IdentityRole { Name = Roles.Admin.ToString(), NormalizedName = Roles.Admin.ToString().ToUpper() },
                    new IdentityRole { Name = Roles.Editor.ToString(), NormalizedName = Roles.Editor.ToString().ToUpper() },
                    new IdentityRole { Name = Roles.Client.ToString(), NormalizedName = Roles.Client.ToString().ToUpper() }
                    );

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var pass = new PasswordHasher<object>().HashPassword(null, "1234");

                context.Users.AddRange(
                    new AppUser
                    {
                        UserName = "noemi",
                        NormalizedUserName = "NOEMI",
                        Email = "noemi@gmail.com",
                        PasswordHash = pass,
                        LockoutEnabled = true,
                    },
                    new AppUser
                    {
                        UserName = "mario",
                        NormalizedUserName = "MARIO",
                        Email = "mario@gmail.com",
                        PasswordHash = pass,
                        LockoutEnabled = true,
                    },
                    new AppUser
                    {
                        UserName = "priscila",
                        NormalizedUserName = "PRISCILA",
                        Email = "priscila@gmail.com",
                        PasswordHash = pass,
                        LockoutEnabled = true,
                    });

                context.SaveChanges();

            }

            if (!context.UserRoles.Any())
            {
                AppUser? adminUser = context.Users.FirstOrDefault(user => user.UserName.Equals("noemi"));
                AppUser? editorUser = context.Users.FirstOrDefault(user => user.UserName.Equals("mario"));
                AppUser? clientUser = context.Users.FirstOrDefault(user => user.UserName.Equals("priscila"));
                IdentityRole? adminRole = context.Roles.FirstOrDefault(rol => rol.Name.Equals(Roles.Admin.ToString()));
                IdentityRole? editorRole = context.Roles.FirstOrDefault(rol => rol.Name.Equals(Roles.Editor.ToString()));
                IdentityRole? clientRole = context.Roles.FirstOrDefault(rol => rol.Name.Equals(Roles.Client.ToString()));

                if (adminUser!= default && editorUser!= default && adminRole != default && editorRole != default)
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string>
                        {
                            RoleId = adminRole.Id,
                            UserId = adminUser.Id,
                        },
                        new IdentityUserRole<string>
                        {
                            RoleId = editorRole.Id,
                            UserId = editorUser.Id,
                        },
                        new IdentityUserRole<string>
                        {
                            RoleId = clientRole.Id,
                            UserId = clientUser.Id,
                        });
                    context.SaveChanges();
                }

            }




            if (!context.Products.Any())
            {
                Category laptop = new Category { Name = "Laptop", Slug = "laptop" };
                Category tv = new Category { Name = "Tv", Slug = "tv" };
                Category nuevo = new Category { Name = "Nuevo", Slug = "nuevo" };

                context.Products.AddRange(
                        new Product
                        {
                            Name = "Nuevo",
                            Slug = "nuevo",
                            Description = "Purple Phone 9",
                            Price = 850M,
                            Category = nuevo,
                            Image = "12.jpg"
                        },
                        new Product
                        {
                            Name = "Nuevo",
                            Slug = "nuevo",
                            Description = "Red Phone",
                            Price = 900M,
                            Category = nuevo,
                            Image = "9.jpg"
                        },
                        new Product
                        {
                            Name = "Nuevo",
                            Slug = "nuevo",
                            Description = "Black Phone 9",
                            Price = 900M,
                            Category = nuevo,
                            Image = "10.jpg"
                        },
                        new Product
                        {
                            Name = "Nuevo",
                            Slug = "nuevo",
                            Description = "Smart watch",
                            Price = 500M,
                            Category = nuevo,
                            Image = "11.jpg"
                        },
                         new Product
                          {
                              Name = "Tv",
                              Slug = "tv",
                              Description = "Pantalla HD",
                              Price = 800M,
                              Category = tv,
                              Image = "4.jpg"
                          },
                          new Product
                          {
                              Name = "Laptop",
                              Slug = "laptop",
                              Description = "Laptop DELL",
                              Price = 5000M,
                              Category = laptop,
                              Image = "laptop2.jpg"
                          },
                           new Product
                           {
                               Name = "Laptop",
                               Slug = "laptop",
                               Description = "Laptop HP-15",
                               Price =5500M,
                               Category = laptop,
                               Image = "laptop3.jpg"
                           }

                );

                context.SaveChanges();
            }

            if (!context.Sales.Any())
            {
                AppUser? clientUser = context.Users.FirstOrDefault(user => user.UserName.Equals("priscila"));
                AppUser? editorUser = context.Users.FirstOrDefault(user => user.UserName.Equals("mario"));
                Product? product = context.Products.FirstOrDefault(prod => prod.Id==1);

                context.Sales.AddRange(new Sale
                {
                    User = clientUser,
                    Datetime = DateTime.Now,
                    Total = 25L,
                    Detail = new DetailSale[]
                    {
                        new DetailSale
                        {
                            Product= product,
                            Price= product.Price,
                            Quantity=5
                        }
                    }

                }, new Sale
                {
                    User = clientUser,
                    Datetime = DateTime.Now,
                    Total = 250L,
                    Detail = new DetailSale[]
                    {
                        new DetailSale
                        {
                            Product= product,
                            Price= product.Price,
                            Quantity=5
                        }
                    }

                }, new Sale
                {
                    User = clientUser,
                    Datetime = DateTime.Now,
                    Total = 425L,
                    Detail = new DetailSale[]
                    {
                        new DetailSale
                        {
                            Product= product,
                            Price= product.Price,
                            Quantity=5
                        }
                    }

                }, new Sale
                {
                    User = editorUser,
                    Datetime = DateTime.Now,
                    Total = 425L,
                    Detail = new DetailSale[]
                    {
                        new DetailSale
                        {
                            Product= product,
                            Price= product.Price,
                            Quantity=5
                        }
                    }

                }
                );
                context.SaveChanges();
            }
        }
    }
}