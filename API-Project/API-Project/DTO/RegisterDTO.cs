using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_Project.DTO
{
    public class RegisterDTO
    {
        [Required]
        [DisplayName("Name")]
        public string UserName {  get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

        [DisplayName("PhoneNumber")]
        public string PhoneNumber {  get; set; }
    }
}
