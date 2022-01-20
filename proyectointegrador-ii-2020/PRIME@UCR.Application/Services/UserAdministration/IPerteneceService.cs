using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IPerteneceService
    {
        Task DeleteUserOfProfileAsync(string idUser, string idProfile);

        Task InsertUserOfProfileAsync(string idUser, string idProfile);
    }
}
