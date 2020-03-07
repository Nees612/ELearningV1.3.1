namespace ELearningV1._3._1.Models
{
    public class Question
    {
        public long Id { get; set; }

        public User User { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
