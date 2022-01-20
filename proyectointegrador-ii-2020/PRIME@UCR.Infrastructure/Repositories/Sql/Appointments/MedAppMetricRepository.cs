using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class MedAppMetricRepository : GenericRepository<MetricasCitaMedica, int>, IMedAppMetricRepository
    {

        public MedAppMetricRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }



        public async Task<MetricasCitaMedica> GetAppMetricsByAppId(int id) { 
            return await _db.MedAppMetrics.FirstOrDefaultAsync(p => p.CitaId == id);
        }

    }
}
