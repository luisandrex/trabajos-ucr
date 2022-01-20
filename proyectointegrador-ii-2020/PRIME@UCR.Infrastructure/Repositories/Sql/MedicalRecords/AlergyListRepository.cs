using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords
{
    public class AlergyListRepository : GenericRepository<ListaAlergia, int>, IAlergyListRepository
    {
        public AlergyListRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }
    }
}
