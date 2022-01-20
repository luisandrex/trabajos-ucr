using System;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class ActionTypeRepository : RepoDbRepository<TipoAccion, string>, IActionTypeRepository
    {
        public ActionTypeRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }
    }
}
