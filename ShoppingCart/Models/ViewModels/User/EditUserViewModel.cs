using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models.ViewModels.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string OldRoleName { get; set; }

        [Required, MinLength(2, ErrorMessage = "You must choose a role")]
        public string NewRoleName { get; set; }
    }
}
