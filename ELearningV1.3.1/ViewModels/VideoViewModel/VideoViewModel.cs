using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearningV1._3._1.ViewModels
{
    public class VideoViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
        [Required]
        public long ModuleContentId { get; set; }
    }
}