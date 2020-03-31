using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearningV1._3._1.Repositories
{
    public class ModuleContentsRepository : Repository<ModuleContent>, IModuleContentsRepository
    {
        public ModuleContentsRepository(DbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ModuleContent>> GetModuleContentsByModuleId(int moduleId)
        {
            return await _dbSet.Where(m => m.Module.Id.Equals(moduleId)).Select(m => new ModuleContent
            {
                Id = m.Id,
                ContentId = m.ContentId,
                Title = m.Title,
                Description = m.Description,
                Lesson = m.Lesson,
                AssigmentUrl = m.AssigmentUrl
            }).ToListAsync();
        }
    }
}
