using System.ComponentModel.DataAnnotations;


namespace ELearningV1._3._1.ViewModels
{
    public class UpdateableUserinfo
    {
        public string OldUserName { get; set; }
        public string OldEmail { get; set; }
        public string OldPhoneNumber { get; set; }
        public string NewUserName { get; set; }
        [EmailAddress(ErrorMessage = "The Email field is invalid.")]
        public string NewEmail { get; set; }
        [Phone(ErrorMessage = "The Phone number field is invalid.")]
        public string NewPhoneNumber { get; set; }
    }
}