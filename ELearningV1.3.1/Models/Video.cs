namespace ELearningV1._3._1.Models
{
    public class Video
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string YoutubeId { get; set; }
        public ModuleContent ModuleContent { get; set; }
    }
}