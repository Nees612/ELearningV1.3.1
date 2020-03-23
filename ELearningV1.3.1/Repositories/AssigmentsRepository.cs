using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ELearningV1._3._1.Repositories
{
    public class AssigmentsRepository : Repository<Assigment>, IAssigmentsRepository
    {
        public AssigmentsRepository(ApiContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Assigment>> GetAssigmentsByModuleName(string moduleName)
        {
            return await _dbSet.Where(a => a.Module.Title.Equals(moduleName)).Select(a => new Assigment
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Url = a.Url
            }).ToListAsync();
        }

        public async Task<IEnumerable<Assigment>> GetAssigmentsWithModules()
        {
            return await _dbSet.Include("Module").Select(a => new Assigment
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Url = a.Url,
                Module = new Module { Id = a.Module.Id, Title = a.Module.Title, Description = a.Module.Description }
            }).ToListAsync();
        }
    }
}
