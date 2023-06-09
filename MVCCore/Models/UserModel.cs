using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVCCore.Models
{
    //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model
    public class UserModel : IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Username")]
        public string CustomUsername { get; set; }

        [ScaffoldColumn(false)]
        public ICollection<ReviewModel>? Reviews { get; set; }
    }
}
