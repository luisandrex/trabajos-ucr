using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Application.Repositories.Multimedia;
using PRIME_UCR.Application.Services.Multimedia;
using System.Linq;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using System;

namespace PRIME_UCR.Application.Implementations.MedicalRecords
{
    internal class ChronicConditionService : IChronicConditionService
    {
        private readonly IChronicConditionRepository _repo;
        private readonly IChronicConditionListRepository _repoLista;
        public ChronicConditionService(IChronicConditionRepository repo, IChronicConditionListRepository repoLista)
        {
            _repo = repo;
            _repoLista = repoLista;
        }
        public async Task<IEnumerable<PadecimientosCronicos>> GetChronicConditionByRecordId(int recordId) 
        {
            return await _repo.GetByConditionAsync(i => i.IdExpediente == recordId);
        }

        public async Task<IEnumerable<ListaPadecimiento>> GetAll()
        {
            return await _repoLista.GetAllAsync();
        }

        public async Task InsertConditionAsync(int recordId, List<ListaPadecimiento> insertList, List<ListaPadecimiento> deleteList)
        {
            if (deleteList.Count > 0)
            {
                foreach (ListaPadecimiento condition in deleteList)
                {
                    await _repo.DeleteByIdsAsync(recordId, condition.Id);
                }
            }

            if (insertList.Count > 0)
            {

                foreach (ListaPadecimiento condition in insertList)
                {
                    PadecimientosCronicos _cond = new PadecimientosCronicos()
                    {
                        IdExpediente = recordId,
                        IdListaPadecimiento = condition.Id,
                        FechaCreacion = DateTime.Now

                    };
                    await _repo.InsertAsync(_cond);
                }

            }
        }
    }
}
