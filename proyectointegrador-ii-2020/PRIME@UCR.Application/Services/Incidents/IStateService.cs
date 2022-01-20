using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.Incidents
{
    public interface IStateService
    {
        Task<IEnumerable<Estado>> GetAllStates();
    }
}
