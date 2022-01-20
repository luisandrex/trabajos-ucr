using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class MedicalSpecialtyRepository : GenericRepository<EspecialidadMedica, string>, IMedicalSpecialtyRepository
    {

        public MedicalSpecialtyRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }

    }
}
