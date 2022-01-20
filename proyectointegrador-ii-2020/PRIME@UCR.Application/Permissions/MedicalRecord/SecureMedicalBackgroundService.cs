using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Permissions.MedicalRecord
{
    public class SecureMedicalBackgroundService : IMedicalBackgroundService
    {
        private readonly IMedicalBackgroundRepository _repo;
        private readonly IMedicalBackgroundListRepository _repoLista;
        private readonly IPrimeSecurityService primeSecurityService;
        private readonly MedicalBackgroundService MedicalBackgroundService;

        public SecureMedicalBackgroundService(IMedicalBackgroundRepository repo,
                                   IMedicalBackgroundListRepository repoLista,
                                   IPrimeSecurityService _primeSecurityService)
        {
            _repo = repo;
            _repoLista = repoLista;
            primeSecurityService = _primeSecurityService;
            MedicalBackgroundService = new MedicalBackgroundService(_repo, repoLista);
        }

        public async Task<IEnumerable<Antecedentes>> GetBackgroundByRecordId(int recordId)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalBackgroundService.GetBackgroundByRecordId(recordId);
        }

        public async Task<IEnumerable<ListaAntecedentes>> GetAll()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalBackgroundService.GetAll();
        }

        public async Task InsertBackgroundAsync(int recordId, List<ListaAntecedentes> insertList, List<ListaAntecedentes> deleteList)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            await MedicalBackgroundService.InsertBackgroundAsync(recordId, insertList, deleteList);
        }
    }
}
