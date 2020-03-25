using System.ComponentModel.DataAnnotations;

namespace ELearningV1._3._1.Models
{
    public class ModuleContent
    {
        public long Id { get; set; }
        public Module Module { get; set; }
        public long ContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssigmentUrl { get; set; }
        [MaxLength(1499)]
        public string Lesson { get; set; }

    }
}
