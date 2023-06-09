using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models.Enumerations;
using MVCCore.Models;

namespace MVCCore.Services
{
    public static class SeedData
    {
        public static void Initalize(IServiceProvider serviceProvider)
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
            context.Games.AddRange(new GameModel
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
            context.SaveChanges();
        }
    }
}
