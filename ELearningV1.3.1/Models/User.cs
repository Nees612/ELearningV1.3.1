using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ELearningV1._3._1.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
