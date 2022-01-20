using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class AppointmentStatusRepository : GenericRepository<EstadoCitaMedica, int>, IAppointmentStatusRepository
    {
        public AppointmentStatusRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }

    }
}
