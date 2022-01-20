using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.DTOs.MedicalRecords;

namespace PRIME_UCR.Application.Repositories.MedicalRecords
{
    public interface IMedicalRecordRepository : IGenericRepository<Expediente, int>
    {
        Task<Expediente> GetByPatientIdAsync(string id);

        Task<IEnumerable<Expediente>> GetByNameAndLastnameAsync(string name, string lastname);

        Task<IEnumerable<Expediente>> GetByNameAndLastnameLastnameAsync(string name, string lastname, string lastname2);

        Task<IEnumerable<Expediente>> GetRecordsWithPatientAsync();

        Task<Expediente> GetDetailsRecordWithPatientDoctorDatesAsync(int id);

        Task<Expediente> UpdateMedicalRecordAsync(Expediente expediente); 
    }

}