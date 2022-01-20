using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IDocumentacionIncidenteRepository : IGenericRepository<DocumentacionIncidente, int>
    {
        Task<IEnumerable<DocumentacionIncidente>> GetAllDocumentationByIncidentCode(string incidentCode);
    }
}


