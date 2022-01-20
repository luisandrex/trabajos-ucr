using PRIME_UCR.Application.Services;
using PRIME_UCR.Application.Repositories;
using PRIME_UCR.Domain.Models;
using System;
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

namespace PRIME_UCR.Application.Implementations.MedicalRecords
{
    internal class AlergyService : IAlergyService
    {
        private readonly IAlergyRepository _repo;
        private readonly IAlergyListRepository _repoLista;
        public AlergyService(IAlergyRepository repo, IAlergyListRepository repoLista)
        {
            _repo = repo;
            _repoLista = repoLista;
        }

        public async Task<IEnumerable<Alergias>> GetAlergyByRecordId(int recordId)
        {
            return await _repo.GetByConditionAsync(i => i.IdExpediente == recordId);
        }

        public async Task<IEnumerable<ListaAlergia>> GetAll() 
        {
            return await _repoLista.GetAllAsync();    
        }

        public async Task InsertAllergyAsync(int recordId, List<ListaAlergia> insertList, List<ListaAlergia> deleteList)
        {
            if (deleteList.Count > 0)
            {
                foreach (ListaAlergia allergy in deleteList)
                {
                    await _repo.DeleteByIdsAsync(recordId, allergy.Id);
                }
            }

            if (insertList.Count > 0)
            {

                foreach (ListaAlergia allergy in insertList)
                {
                    Alergias _al = new Alergias()
                    {
                        IdExpediente = recordId,
                        IdListaAlergia = allergy.Id,
                        FechaCreacion = DateTime.Now

                    };
                    await _repo.InsertAsync(_al);
                }

            }
        }
    }
}
