using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;

namespace ELearningV1._3._1.Repositories
{
    public class ModulesRepository : Repository<Module>, IModulesRepository
    {
        public ModulesRepository(ApiContext context) : base(context)
        {
        }
    }
}
