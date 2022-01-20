using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.UserAdministration
{
    public interface IPersonaRepository
    {
        Task<Persona> GetByCedPersonaAsync(string ced);

        Task<Persona> GetByKeyPersonaAsync(string id);

        Task<Persona> GetWithDetailsAsync(string id);

        Task InsertAsync(Persona persona);

        Task DeleteAsync(string cedPersona);
    }
}
