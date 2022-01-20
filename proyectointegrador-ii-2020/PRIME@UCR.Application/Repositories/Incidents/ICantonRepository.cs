using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface ICantonRepository : IGenericRepository<Canton, int>
    {
        Task<IEnumerable<Canton>> GetCantonsByProvinceNameAsync(string provinceName);
    }
}