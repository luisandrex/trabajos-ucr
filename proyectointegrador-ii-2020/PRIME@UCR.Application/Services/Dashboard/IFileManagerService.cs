using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.Dashboard
{
    public interface IFileManagerService
    {
        Task createFileAsync(List<Incidente> filteredIncidentsData, string userIdentifier, string userName);
    }
}
