using System.ComponentModel.DataAnnotations;


namespace ELearningV1._3._1.ViewModels
{
    public class UserUpdateViewModel
    {

        [Required]
        [MinLength(6, ErrorMessage = "The User name field is inavlid, user name must be longer that 6, can be made of letters and numbers.")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "The Email field is invalid.")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "The Phone number field is invalid.")]
        public string PhoneNumber { get; set; }
    }
}