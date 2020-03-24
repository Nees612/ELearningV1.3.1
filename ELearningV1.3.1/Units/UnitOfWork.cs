using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Repositories;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Units
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext _context;
        public UnitOfWork(ApiContext context)
        {
            _context = context;
            Users = new UsersRepository(_context);
            Assigments = new AssigmentsRepository(_context);
            Modules = new ModulesRepository(_context);
            ModuleContents = new ModuleContentsRepository(_context);
        }
        public IUsersRepository Users { get; private set; }
        public IAssigmentsRepository Assigments { get; private set; }
        public IModulesRepository Modules { get; private set; }
        public IModuleContentsRepository ModuleContents { get; private set; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
