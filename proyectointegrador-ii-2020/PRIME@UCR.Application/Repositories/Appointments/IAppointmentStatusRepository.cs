using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IAppointmentStatusRepository : IGenericRepository<EstadoCitaMedica, int>
    {
    }
}
