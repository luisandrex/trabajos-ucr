using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IPermiteService
    {
        Task DeletePermissionAsync(string idProfile, int idPermission);

        Task InsertPermissionAsync(string idProfile, int idPermission);
    }
}
