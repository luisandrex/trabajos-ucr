using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IAppointmentReferenceRepository : IGenericRepository<ReferenciaCita, int>
    {

        Task InsertReference(ReferenciaCita reference);

        Task<IEnumerable<ReferenciaCita>> GetReferencesByAppId(int id);

    }
}
