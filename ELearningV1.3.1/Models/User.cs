using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ELearningV1._3._1.Models
{
    public class User : IdentityUser
    {
        [Required]
        public override string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
