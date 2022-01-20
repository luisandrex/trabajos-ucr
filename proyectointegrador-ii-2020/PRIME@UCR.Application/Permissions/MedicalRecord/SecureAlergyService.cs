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
    public class SecureAlergyService : IAlergyService
    {
        private readonly IAlergyRepository _repo;
        private readonly IAlergyListRepository _repoLista;
        private readonly IPrimeSecurityService primeSecurityService;
        private readonly AlergyService AllergyService;

        public SecureAlergyService(IAlergyRepository repo,
                                   IAlergyListRepository repoLista,
                                   IPrimeSecurityService _primeSecurityService)
        {
            _repo = repo;
            _repoLista = repoLista;
            primeSecurityService = _primeSecurityService;
            AllergyService = new AlergyService(_repo, repoLista);
        }

        public async Task<IEnumerable<Alergias>> GetAlergyByRecordId(int recordId)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await AllergyService.GetAlergyByRecordId(recordId);
        }

        public async Task<IEnumerable<ListaAlergia>> GetAll()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await AllergyService.GetAll();
        }

        public async Task InsertAllergyAsync(int recordId, List<ListaAlergia> insertList, List<ListaAlergia> deleteList)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            await AllergyService.InsertAllergyAsync(recordId, insertList, deleteList);
        }
    }
}
