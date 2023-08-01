using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models.ViewModels.User
{
    public class CreateUserViewModel
    {
        public string Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }

        public string RoleId { get; set; }

        [Required, MinLength(2, ErrorMessage = "You must choose a role")]
        public string RoleName { get; set; }
    }
}
