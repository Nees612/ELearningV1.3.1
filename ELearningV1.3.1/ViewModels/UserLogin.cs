using System.ComponentModel.DataAnnotations;


namespace ELearningV1._3._1.ViewModels
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}