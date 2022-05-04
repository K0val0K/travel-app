using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Travel Agencies
                if (!context.TravelAgencies.Any())
                {
                    context.TravelAgencies.AddRange(new List<TravelAgency>()
                    {
                        new TravelAgency()
                        {
                            Name = "Travel Agency 1",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-1.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new TravelAgency()
                        {
                            Name = "Travel Agency 2",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-2.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new TravelAgency()
                        {
                            Name = "Travel Agency 3",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-3.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new TravelAgency()
                        {
                            Name = "Travel Agency 4",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-4.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new TravelAgency()
                        {
                            Name = "Travel Agency 5",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-5.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                    });
                    context.SaveChanges();
                }
                //Countries
                if (!context.Countries.Any())
                {
                    context.Countries.AddRange(new List<Country>()
                    {
                        new Country()
                        {
                            CountryName = "Country 1",
                            Description = "This is the Bio of the first actor",
                            CountryPictureURL = "http://dotnethow.net/images/actors/actor-1.jpeg"

                        },
                        new Country()
                        {
                            CountryName = "Country 2",
                            Description = "This is the Bio of the second actor",
                            CountryPictureURL = "http://dotnethow.net/images/actors/actor-2.jpeg"
                        },
                        new Country()
                        {
                            CountryName = "Country 3",
                            Description = "This is the Bio of the second actor",
                            CountryPictureURL = "http://dotnethow.net/images/actors/actor-3.jpeg"
                        },
                        new Country()
                        {
                            CountryName = "Country 4",
                            Description = "This is the Bio of the second actor",
                            CountryPictureURL = "http://dotnethow.net/images/actors/actor-4.jpeg"
                        },
                        new Country()
                        {
                            CountryName = "Country 5",
                            Description = "This is the Bio of the second actor",
                            CountryPictureURL = "http://dotnethow.net/images/actors/actor-5.jpeg"
                        }
                    });
                    context.SaveChanges();
                }

                //Tours
                if (!context.Tours.Any())
                {
                    context.Tours.AddRange(new List<Tour>()
                    {
                        new Tour()
                        {
                            Name = "Life",
                            Description = "This is the Life movie description",
                            Price = 39.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-3.jpeg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(10),
                            TravelAgencyId = 3,
                            TourCategory = TourCategory.AllInclusive
                        },
                        new Tour()
                        {
                            Name = "The Shawshank Redemption",
                            Description = "This is the Shawshank Redemption description",
                            Price = 29.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-1.jpeg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(3),
                            TravelAgencyId = 1,
                            TourCategory = TourCategory.Excursion
                        },
                        new Tour()
                        {
                            Name = "Ghost",
                            Description = "This is the Ghost movie description",
                            Price = 39.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-4.jpeg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(7),
                            TravelAgencyId = 4,
                            TourCategory = TourCategory.Shopping
                        },
                        new Tour()
                        {
                            Name = "Race",
                            Description = "This is the Race movie description",
                            Price = 39.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-6.jpeg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(-5),
                            TravelAgencyId = 1,
                            TourCategory = TourCategory.Shopping
                        },
                        new Tour()
                        {
                            Name = "Scoob",
                            Description = "This is the Scoob movie description",
                            Price = 39.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-7.jpeg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(-2),
                            TravelAgencyId = 1,
                            TourCategory = TourCategory.Individual
                        },
                        new Tour()
                        {
                            Name = "Cold Soles",
                            Description = "This is the Cold Soles movie description",
                            Price = 39.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-8.jpeg",
                            StartDate = DateTime.Now.AddDays(3),
                            EndDate = DateTime.Now.AddDays(20),
                            TravelAgencyId = 1,
                            TourCategory = TourCategory.Shopping
                        }
                    });
                    context.SaveChanges();
                }
                //Countries & Tours
                if (!context.Countries_Tours.Any())
                {
                    context.Countries_Tours.AddRange(new List<Country_Tour>()
                    {
                        new Country_Tour()
                        {
                            CountryId = 1,
                            TourId = 1
                        },
                        new Country_Tour()
                        {
                            CountryId = 3,
                            TourId = 1
                        },

                         new Country_Tour()
                        {
                            CountryId = 1,
                            TourId = 2
                        },
                         new Country_Tour()
                        {
                            CountryId = 4,
                            TourId = 2
                        },

                        new Country_Tour()
                        {
                            CountryId = 1,
                            TourId = 3
                        },
                        new Country_Tour()
                        {
                            CountryId = 2,
                            TourId = 3
                        },
                        new Country_Tour()
                        {
                            CountryId = 5,
                            TourId = 3
                        },


                        new Country_Tour()
                        {
                            CountryId = 2,
                            TourId = 4
                        },
                        new Country_Tour()
                        {
                            CountryId = 3,
                            TourId = 4
                        },
                        new Country_Tour()
                        {
                            CountryId = 4,
                            TourId = 4
                        },


                        new Country_Tour()
                        {
                            CountryId = 2,
                            TourId = 5
                        },
                        new Country_Tour()
                        {
                            CountryId = 3,
                            TourId = 5
                        },
                        new Country_Tour()
                        {
                            CountryId = 4,
                            TourId = 5
                        },
                        new Country_Tour()
                        {
                            CountryId = 5,
                            TourId = 5
                        },


                        new Country_Tour()
                        {
                            CountryId = 3,
                            TourId = 6
                        },
                        new Country_Tour()
                        {
                            CountryId = 4,
                            TourId = 6
                        },
                        new Country_Tour()
                        {
                            CountryId = 5,
                            TourId = 6
                        },
                    });
                    context.SaveChanges();
                }
            }

        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@etickets.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if(adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
