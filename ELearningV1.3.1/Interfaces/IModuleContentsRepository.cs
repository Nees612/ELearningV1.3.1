using ELearningV1._3._1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Interfaces
{
    public interface IModuleContentsRepository : IRepository<ModuleContent>
    {
        Task<IEnumerable<ModuleContent>> GetModuleContentsByModuleId(long id);
    }
}
