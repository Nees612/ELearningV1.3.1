using System.ComponentModel.DataAnnotations;

namespace ELearningV1._3._1.Models
{
    public class Module
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
