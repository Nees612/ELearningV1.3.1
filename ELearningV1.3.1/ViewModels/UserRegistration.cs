using System.ComponentModel.DataAnnotations;

namespace ELearningV1._3._1.ViewModels
{
    public class UserRegistration
    {
        [Required]
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