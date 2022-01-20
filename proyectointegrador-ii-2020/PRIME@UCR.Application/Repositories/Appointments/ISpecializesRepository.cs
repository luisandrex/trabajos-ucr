using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface ISpecializesRepository : IGenericRepository<SeEspecializa, string>
    {
        Task<IEnumerable<Persona>> GetDoctorsWithSpecialty(string specialty_name);

    }
}
