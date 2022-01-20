using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IActionRepository
    {
        Task<Accion> InsertAsync(Accion action);
        Task DeleteAsync(int citaId, string nombreAccion, int mcId);
        Task<IEnumerable<MultimediaContent>> GetByAppointmentAction(int citaId, string nombreAccion);
    }
}
