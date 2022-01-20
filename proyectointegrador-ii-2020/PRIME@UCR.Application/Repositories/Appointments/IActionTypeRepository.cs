using System;
using PRIME_UCR.Application.Repositories;
using PRIME_UCR.Domain.Models.Appointments;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IActionTypeRepository : IGenericRepository<TipoAccion, string>
    {
    }
}
