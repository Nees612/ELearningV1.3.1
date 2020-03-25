using ELearningV1._3._1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Interfaces
{
    public interface IVideosRepository : IRepository<Video>
    {
        Task<IEnumerable<Video>> GetVideosByModuleContentId(long Id);
    }
}