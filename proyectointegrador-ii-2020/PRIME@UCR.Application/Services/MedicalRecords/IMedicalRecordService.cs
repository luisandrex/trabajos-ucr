using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Services.MedicalRecords
{
    public interface IMedicalRecordService
    {
        // returns null if no such record exists
        Task<Expediente> GetByPatientIdAsync(string id);

        Task<IEnumerable<Expediente>> GetAllAsync();

        Task<IEnumerable<Expediente>> GetByConditionAsync(string name);

        Task<Expediente> InsertAsync(Expediente expediente);

        Task<Expediente> CreateMedicalRecordAsync(Expediente entity);

        Task<IEnumerable<Paciente>> GetPatients();

        Task<IEnumerable<Funcionario>> GetMedics();
        Task<RecordViewModel> GetIncidentDetailsAsync(int identification);

        Task<Expediente> GetMedicalRecordDetailsLinkedAsync(int identification); 

        Task<IEnumerable<Incidente>> GetMedicalRecordIncidents(int recordId);

        Task<IEnumerable<CentroMedico>> GetMedicalCentersAsync();

        Task<IEnumerable<Antecedentes>> GetBackgroundByRecordId(int recordId);

        Task<IEnumerable<ListaAntecedentes>> GetAll();

        Task<IEnumerable<Alergias>> GetAlergyByRecordId(int recordId);

        Task<IEnumerable<ListaAlergia>> GetAllAlergies();

        Task<Expediente> UpdateMedicalRecordAsync(Expediente expediente);

        Task<CentroMedico> GetMedicalCenterByUbicationCenterIdAsync(int id); 

    }
}