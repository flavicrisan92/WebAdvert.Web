using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "EmailAddress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, ErrorMessage = "Password must be at least six characters long!")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and it's confirmation does not match")]
        public string ConfirmPassword { get; set; }
    }
}
