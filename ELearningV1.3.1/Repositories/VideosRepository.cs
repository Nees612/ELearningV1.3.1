using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Repositories
{
    public class VideosRepository : Repository<Video>, IVideosRepository
    {

        public VideosRepository(ApiContext context) : base(context) { }

        public async Task<IEnumerable<Video>> GetVideosByModuleContentId(long Id)
        {
            return await _dbSet.Where(v => v.ModuleContent.Id.Equals(Id)).Select(v => new Video
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Description,
                Url = v.Url,
                YoutubeId = v.YoutubeId
            }).ToListAsync();
        }
    }

}