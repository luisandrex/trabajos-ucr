using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IStateRepository : IGenericRepository<Estado, string>
    {
        Task<IEnumerable<Estado>> GetAllStates();
    }
}