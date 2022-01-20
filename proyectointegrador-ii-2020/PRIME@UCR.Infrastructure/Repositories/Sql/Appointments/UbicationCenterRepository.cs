using PRIME_UCR.Application.Repositories.Appointments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class UbicationCenterRepository : GenericRepository<CentroUbicacion, int>, IUbicationCenterRepository
    {

        public UbicationCenterRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }


    }
}
