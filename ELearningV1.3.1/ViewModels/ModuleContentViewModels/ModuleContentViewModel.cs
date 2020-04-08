using System.ComponentModel.DataAnnotations;

namespace ELearningV1._3._1.ViewModels
{
    public class ModuleContentViewModel
    {
        [Required]
        public long ModuleId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Url]
        public string AssigmentUrl { get; set; }
        [Required]
        [MaxLength(1499)]
        public string Lesson { get; set; }
    }
}