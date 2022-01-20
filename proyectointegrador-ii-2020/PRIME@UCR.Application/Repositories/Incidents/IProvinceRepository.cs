using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IProvinceRepository : IGenericRepository<Provincia, string>
    {
        Task<IEnumerable<Provincia>> GetProvincesByCountryNameAsync(string countryName);
    }
}