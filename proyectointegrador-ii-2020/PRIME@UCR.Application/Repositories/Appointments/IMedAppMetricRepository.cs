using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IMedAppMetricRepository : IGenericRepository<MetricasCitaMedica, int>
    {
        Task<MetricasCitaMedica> GetAppMetricsByAppId(int id); 

    }
}
