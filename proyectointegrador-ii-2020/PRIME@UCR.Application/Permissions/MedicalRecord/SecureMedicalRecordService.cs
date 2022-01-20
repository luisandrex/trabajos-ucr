using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Domain.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.MedicalRecords;

namespace PRIME_UCR.Application.Permissions.MedicalRecord
{
    public class SecureMedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _repo;
        private readonly IPersonaRepository _personRepo;
        private readonly IPacienteRepository _patientrepo;
        private readonly IFuncionarioRepository _medicrepo;
        private readonly IIncidentRepository _incidentrepo;
        private readonly IMedicalCenterRepository _mcrepo;
        private readonly IMedicalBackgroundRepository _mbrepo;
        private readonly IAlergyRepository _arepo;
        private readonly IAlergyListRepository _larepo;
        private readonly IMedicalBackgroundListRepository _lantrepo;
        private readonly IUbicationCenterRepository _ubcrepo;
        private readonly IMedCenterRepository _medcenrepo;
        private readonly IPrimeSecurityService primeSecurityService;
        private readonly MedicalRecordService MedicalRecordService;

        public SecureMedicalRecordService(IMedicalRecordRepository repo,
                                    IPersonaRepository personRepo,
                                    IPacienteRepository repo1,
                                    IFuncionarioRepository repo2,
                                    IIncidentRepository incidentrepo,
                                    IMedicalCenterRepository mcs,
                                    IMedicalBackgroundRepository mbs,
                                    IAlergyRepository als,
                                    IAlergyListRepository alls,
                                    IMedicalBackgroundListRepository mbls,
                                    IUbicationCenterRepository ubc,
                                    IMedCenterRepository mcr,
                                    IPrimeSecurityService _primeSecurityService)
        {
            _repo = repo;
            _personRepo = personRepo;
            _patientrepo = repo1;
            _medicrepo = repo2;
            _incidentrepo = incidentrepo;
            _mcrepo = mcs;
            _mbrepo = mbs;
            _arepo = als;
            _larepo = alls;
            _lantrepo = mbls;
            _ubcrepo = ubc;
            _medcenrepo = mcr;
            primeSecurityService = _primeSecurityService;
            MedicalRecordService = new MedicalRecordService(_repo, _personRepo, _patientrepo, _medicrepo, _incidentrepo, _mcrepo, _mbrepo, _arepo, _larepo, _lantrepo, _ubcrepo, _medcenrepo);
        }

        public async Task<IEnumerable<CentroMedico>> GetMedicalCentersAsync()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetMedicalCentersAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByConditionAsync(string name)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetByConditionAsync(name);
        }

        public async Task<IEnumerable<Expediente>> GetAllAsync()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetAllAsync();
        }

        public async Task<CentroMedico> GetMedicalCenterByUbicationCenterIdAsync(int id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetMedicalCenterByUbicationCenterIdAsync(id);
        }

        public async Task<IEnumerable<Paciente>> GetPatients()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetPatients();
        }

        public async Task<IEnumerable<Funcionario>> GetMedics()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetMedics();
        }

        public async Task<Expediente> GetByPatientIdAsync(string id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetByPatientIdAsync(id);
        }

        public async Task<Expediente> InsertAsync(Expediente expediente)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.InsertAsync(expediente);
        }

        public async Task<Expediente> CreateMedicalRecordAsync(Expediente entity)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.CreateMedicalRecordAsync(entity);
        }

        public async Task<Expediente> GetByIdAsync(int id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Incidente>> GetMedicalRecordIncidents(int recordId)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetMedicalRecordIncidents(recordId);
        }

        public async Task<Expediente> GetMedicalRecordDetailsLinkedAsync(int identification)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetMedicalRecordDetailsLinkedAsync(identification);
        }

        public async Task<Expediente> UpdateMedicalRecordAsync(Expediente expediente)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.UpdateMedicalRecordAsync(expediente);
        }

        public async Task<RecordViewModel> GetIncidentDetailsAsync(int id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetIncidentDetailsAsync(id);
        }

        public async Task<IEnumerable<Antecedentes>> GetBackgroundByRecordId(int recordId)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetBackgroundByRecordId(recordId);
        }

        public async Task<IEnumerable<Alergias>> GetAlergyByRecordId(int recordId)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetAlergyByRecordId(recordId);
        }

        public async Task<IEnumerable<ListaAlergia>> GetAllAlergies()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetAllAlergies();
        }

        public async Task<IEnumerable<ListaAntecedentes>> GetAll()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeAllMedicalRecords, AuthorizationPermissions.CanSeeMedicalRecordsOfHisPatients });
            return await MedicalRecordService.GetAll();
        }
    }

}