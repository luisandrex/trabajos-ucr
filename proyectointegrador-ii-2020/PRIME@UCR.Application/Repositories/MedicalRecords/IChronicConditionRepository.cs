using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;

namespace PRIME_UCR.Application.Repositories.MedicalRecords
{
    public interface IChronicConditionRepository : IGenericRepository<PadecimientosCronicos, int>
    {
        Task DeleteByIdsAsync(int recordId, int listId);
    }
}
