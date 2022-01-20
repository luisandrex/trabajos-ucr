using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Services.MedicalRecords
{
    public interface IChronicConditionService
    {
        Task<IEnumerable<PadecimientosCronicos>> GetChronicConditionByRecordId(int recordId);

        Task<IEnumerable<ListaPadecimiento>> GetAll();

        Task InsertConditionAsync(int recordId, List<ListaPadecimiento> insertedList, List<ListaPadecimiento> deletedList);

    }
}
