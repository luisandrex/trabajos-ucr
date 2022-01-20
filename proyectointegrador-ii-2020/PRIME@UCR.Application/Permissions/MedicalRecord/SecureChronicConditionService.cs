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
    public class SecureChronicConditionService : IChronicConditionService
    {
        private readonly IChronicConditionRepository _repo;
        private readonly IChronicConditionListRepository _repoLista;
        private readonly IPrimeSecurityService primeSecurityService;
        private readonly ChronicConditionService ChronicConditionService;

        public SecureChronicConditionService(IChronicConditionRepository repo,
                                   IChronicConditionListRepository repoLista,
                                   IPrimeSecurityService _primeSecurityService)
        {
            _repo = repo;
            _repoLista = repoLista;
            primeSecurityService = _primeSecurityService;
            ChronicConditionService = new ChronicConditionService(_repo, repoLista);
        }

        public async Task<IEnumerable<PadecimientosCronicos>> GetChronicConditionByRecordId(int recordId)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await ChronicConditionService.GetChronicConditionByRecordId(recordId);
        }

        public async Task<IEnumerable<ListaPadecimiento>> GetAll()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await ChronicConditionService.GetAll();
        }

        public async Task InsertConditionAsync(int recordId, List<ListaPadecimiento> insertList, List<ListaPadecimiento> deleteList)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            await ChronicConditionService.InsertConditionAsync(recordId, insertList, deleteList);
        }
    }
}
