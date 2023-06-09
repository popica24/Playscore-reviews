using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVCCore.Models
{
    public class ReviewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [DisplayName("Leave your review")]
        public string Review { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Date added")]
        public DateTime DateAdded { get; set; }

        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }

        [Required]
        public Guid GameId { get; set; }

        public GameModel Game { get; set; }

        [Required]
        public string UserId { get; set; }
    
        public UserModel User { get; set; }
    }
}
