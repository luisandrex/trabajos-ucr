using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.DTOs.MedicalRecords;

namespace PRIME_UCR.Application.Repositories.MedicalRecords
{
    public interface IAlergyRepository : IGenericRepository<Alergias, int>
    {
        Task DeleteByIdsAsync(int recordId, int listId);
    }
}
