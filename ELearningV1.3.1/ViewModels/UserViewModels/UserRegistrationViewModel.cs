using System.ComponentModel.DataAnnotations;

namespace ELearningV1._3._1.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required]
        [MinLength(6, ErrorMessage = "Username must be longer that 6 characters, can be made of letters and numbers.")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}