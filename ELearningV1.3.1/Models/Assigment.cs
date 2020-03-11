namespace ELearningV1._3._1.Models
{
    public class Assigment
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public Module Module { get; set; }
    }
}