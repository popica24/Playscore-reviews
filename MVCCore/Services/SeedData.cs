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

            string email = "admin@playscore.com";
            string password = "Admin1234_";

            if(await context.FindByEmailAsync(email) == null)
            {
                var user = new UserModel();
                user.CustomUsername = "Admin";
                user.UserName = email;
                user.Email = email;
                user.EmailConfirmed = true;

               await context.CreateAsync(user, password);
                var role = RoleEnumeration.Admin.ToString();
                await context.AddToRoleAsync(user,role);
            }
        }
    }
}
