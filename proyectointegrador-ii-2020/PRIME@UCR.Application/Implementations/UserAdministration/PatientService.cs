using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using StringFreezingAttribute = System.Runtime.CompilerServices.StringFreezingAttribute;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    public class PatientService : IPatientService
    {
        private readonly IPacienteRepository _patientRepo;
        private readonly IEncryptionService _encryptionService;

        public PatientService(IPacienteRepository patientRepo, IEncryptionService encryptionService)
        {
            _patientRepo = patientRepo;
            _encryptionService = encryptionService;
        }

        private Paciente DecryptPatient(Paciente patient)
        {
            patient.Nombre = _encryptionService.Decrypt(Convert.FromBase64String(patient.Nombre));
            patient.PrimerApellido = _encryptionService.Decrypt(Convert.FromBase64String(patient.PrimerApellido));
            patient.Cédula = _encryptionService.Decrypt(Convert.FromBase64String(patient.PrimerApellido));

            patient.SegundoApellido = patient.SegundoApellido is null
                ? null
                : _encryptionService.Decrypt(Convert.FromBase64String(patient.SegundoApellido));

            return patient;
        }

        private Paciente EncryptPatient(Paciente patient)
        {
            patient.Nombre = Convert.ToBase64String(_encryptionService.Encrypt(patient.Nombre));
            patient.PrimerApellido = Convert.ToBase64String(_encryptionService.Encrypt(patient.PrimerApellido));
            patient.Cédula = Convert.ToBase64String(_encryptionService.Encrypt(patient.Cédula));

            patient.SegundoApellido = patient.SegundoApellido is null
                ? null
                : Convert.ToBase64String(_encryptionService.Encrypt(patient.SegundoApellido));

            return patient;
        }

        public async Task<Paciente> GetPatientByIdAsync(string id)
        {
            return await _patientRepo.GetByKeyAsync(id);
        }

        public async Task<Paciente> CreatePatientAsync(Paciente entity)
        {
            return await _patientRepo.InsertAsync(entity);
        }

        public async Task<Paciente> InsertPatientOnlyAsync(Paciente entity)
        {
            return await _patientRepo.InsertPatientOnlyAsync(entity);
        }
    }
}
