using ELearningV1._3._1.Interfaces;

namespace ELearningV1._3._1.Managers
{
    public class VideoManager : IVideoManager
    {
        public VideoManager() { }

        public string ConvertUrl(string Url)
        {
            return Url.Replace("watch?v=", "embed/");
        }

        public string GetYoutubeId(string Url)
        {
            var s = Url.Split('/');
            return s[s.Length - 1];
        }
    }
}