using ELearningV1._3._1.Models;
using System.ComponentModel.DataAnnotations;

namespace ELearningV1._3._1.ViewModels
{
    public class AssigmentViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
        [Required]
        public long ModuleId { get; set; }
    }
}
