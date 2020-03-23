using ELearningV1._3._1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Interfaces
{
    public interface IAssigmentsRepository : IRepository<Assigment>
    {
        Task<IEnumerable<Assigment>> GetAssigmentsWithModules();

        Task<IEnumerable<Assigment>> GetAssigmentsByModuleName(string moduleName);
    }
}
