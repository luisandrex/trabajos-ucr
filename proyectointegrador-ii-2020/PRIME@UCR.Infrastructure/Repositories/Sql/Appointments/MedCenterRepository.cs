using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    class MedCenterRepository : GenericRepository<CentroMedico, int>, IMedCenterRepository
    {
        public MedCenterRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }


    }
}
