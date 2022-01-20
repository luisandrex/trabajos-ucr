using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.Multimedia;

namespace PRIME_UCR.Application.Permissions.UserAdministration
{

    public class SecurePatientService : IPatientService
    {
        private readonly IPacienteRepository patientRepo;
        private readonly IEncryptionService encryptionService;

        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PatientService patientService;

        public SecurePatientService(IPacienteRepository _patientRepo, IEncryptionService encryptionService,
            IPrimeSecurityService _primeSecurityService)
        {
            patientRepo = _patientRepo;
            primeSecurityService = _primeSecurityService;
            patientService = new PatientService(patientRepo, encryptionService);
        }

        public async Task<Paciente> GetPatientByIdAsync(string id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeInfoOfIncidentsPatient });
            return await patientService.GetPatientByIdAsync(id);
        }

        public async Task<Paciente> CreatePatientAsync(Paciente entity)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanEditMedicalInfoOfIncidentsPatient });
            return await patientService.CreatePatientAsync(entity);
        }

        public async Task<Paciente> InsertPatientOnlyAsync(Paciente entity)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanEditMedicalInfoOfIncidentsPatient });
            return await patientService.InsertPatientOnlyAsync(entity);
        }
    }
}
