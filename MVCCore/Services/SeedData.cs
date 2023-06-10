using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models.Enumerations;
using MVCCore.Models;
using Microsoft.AspNetCore.Identity;

namespace MVCCore.Services
{
    public static class SeedData
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            await InitalizeDbData(serviceProvider);
            await InitializeDbRoles(serviceProvider);
            await InitializeDbUsers(serviceProvider);
            await InitializeReviews(serviceProvider);
        }

        private static async Task InitalizeDbData(IServiceProvider serviceProvider)
        {
            using var context = new PlayscoreDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<PlayscoreDbContext>>());
            if(context == null || context.Games == null)
            {
                throw new ArgumentNullException("Null context");
            }
            if (context.Games.Any())
            {
                return;
            }
            await context.Games.AddRangeAsync(new GameModel
            {
                Name = "Call of Duty: Modern Warfare",
                Description = "A first-person shooter game set in a modern warfare setting.",
                Category = GameCategory.FPS,
                AgeRating = AgeRating.PEGI18
            },
    new GameModel
    {
        Name = "The Witcher 3: Wild Hunt",
        Description = "An open-world RPG game based on the Witcher book series.",
        Category = GameCategory.RPG,
        AgeRating = AgeRating.PEGI18
    },
    new GameModel
    {
        Name = "World of Warcraft",
        Description = "A massively multiplayer online role-playing game set in a fantasy world.",
        Category = GameCategory.MMO,
        AgeRating = AgeRating.PEGI15
    },
    new GameModel
    {
        Name = "League of Legends",
        Description = "A popular multiplayer online battle arena game.",
        Category = GameCategory.MOBA,
        AgeRating = AgeRating.PEGI12
    },
    new GameModel
    {
        Name = "Assassin's Creed Valhalla",
        Description = "An action-adventure game featuring Vikings.",
        Category = GameCategory.RPG,
        AgeRating = AgeRating.PEGI18
    },
    new GameModel
    {
        Name = "FIFA 22",
        Description = "A football simulation game with realistic gameplay.",
        Category = GameCategory.Sports,
        AgeRating = AgeRating.PEGI3
    },
    new GameModel
    {
        Name = "Minecraft",
        Description = "A sandbox game that allows players to build and explore virtual worlds.",
        Category = GameCategory.Sandbox,
        AgeRating = AgeRating.PEGI7
    },
    new GameModel
    {
        Name = "StarCraft II",
        Description = "A real-time strategy game set in a science fiction universe.",
        Category = GameCategory.RTS,
        AgeRating = AgeRating.PEGI16
    },
    new GameModel
    {
        Name = "Grand Theft Auto V",
        Description = "An open-world action-adventure game.",
        Category = GameCategory.TPS,
        AgeRating = AgeRating.PEGI18
    },
    new GameModel
    {
        Name = "The Legend of Zelda: Breath of the Wild",
        Description = "An action-adventure game featuring the iconic character Link.",
        Category = GameCategory.RPG,
        AgeRating = AgeRating.PEGI12
    });
          await context.SaveChangesAsync();
        }
  
        private static async Task InitializeDbRoles(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await context.RoleExistsAsync(role))
                    await context.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task InitializeDbUsers(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<UserManager<UserModel>>();

            string password = "FOOfoo1_";

            for (int i = 1; i <= 10; i++)
            {
                string userEmail = $"user{i}@playscore.com";

                if (await context.FindByEmailAsync(userEmail) == null)
                {
                    var user = new UserModel();
                    user.CustomUsername = $"User{i}";
                    user.UserName = userEmail;
                    user.Email = userEmail;
                    user.EmailConfirmed = true;

                    await context.CreateAsync(user, password);
                    if (i == 1)
                    {
                        user.UserName = "admin@playscore.com";
                        user.Email = "admin@playscore.com";
                        var role = RoleEnumeration.Admin.ToString();
                        await context.AddToRoleAsync(user, role);
                    }
                    else {
                        var role = RoleEnumeration.User.ToString();
                        await context.AddToRoleAsync(user, role);
                    }
                }
            }

        }
  
        private static async Task InitializeReviews(IServiceProvider serviceProvider)
        {
            using var context = new PlayscoreDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<PlayscoreDbContext>>());
            var game = await context.Games.Include(r => r.Reviews).SingleOrDefaultAsync(x => x.Name == "World of Warcraft");

            if (!game.Reviews.Any())
            {
                List<ReviewModel> reviewsList = new List<ReviewModel>();
                Random random = new();
                foreach (var user in await context.Users.ToListAsync())
                {
                    string userEmail = user.UserName;
                    var review = new ReviewModel();
                    review.Username = userEmail;
                    review.Review = $"This is reviewed by {user.CustomUsername}.";
                    review.DateAdded = DateTime.Now;
                    review.Stars = random.Next(1, 6);
                    review.GameId = game.Id;
                    review.UserId = user.Id;

                    reviewsList.Add(review);
                }


                await context.Reviews.AddRangeAsync(reviewsList);
                await context.SaveChangesAsync();
            }
        }
    }
}
