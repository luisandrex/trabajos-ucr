using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Dashboard
{
    public interface IDashboardRepository : IGenericRepository<Incidente, string>
    {
        Task<List<Incidente>> GetAllIncidentsAsync();

        Task<List<Distrito>> GetAllDistrictsAsync();
        
        Task<int> GetIncidentsCounterAsync(string modality, string filter);

        Task<List<Paciente>> GetAllPacientes();
    }
}
