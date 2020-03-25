using System.Threading.Tasks;

namespace ELearningV1._3._1.Interfaces
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }
        IAssigmentsRepository Assigments { get; }
        IModulesRepository Modules { get; }
        IModuleContentsRepository ModuleContents { get; }
        IVideosRepository Videos { get; }

        Task<int> Complete();
    }
}
