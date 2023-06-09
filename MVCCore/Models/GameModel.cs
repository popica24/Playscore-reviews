using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MVCCore.Models.Enumerations;
using MVCCore.Services;

namespace MVCCore.Models
{
    public class GameModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Game name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public GameCategory Category { get; set; }

        [Required]
        public AgeRating AgeRating { get; set; }

        public PaginatedList<ReviewModel>? Reviews { get; set; }
    }
}
