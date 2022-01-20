using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;

namespace PRIME_UCR.Application.Implementations.MedicalRecords
{
    internal class MedicalBackgroundService : IMedicalBackgroundService
    {
        private readonly IMedicalBackgroundRepository _repo;
        private readonly IMedicalBackgroundListRepository _repoLista;
        public MedicalBackgroundService(IMedicalBackgroundRepository repo, IMedicalBackgroundListRepository repoLista)
        {
            _repo = repo;
            _repoLista = repoLista;
        }

        public async Task<IEnumerable<Antecedentes>> GetBackgroundByRecordId(int recordId)
        {
            return await _repo.GetByConditionAsync(i => i.IdExpediente == recordId);
        }
        public async Task<IEnumerable<ListaAntecedentes>> GetAll()
        {
            return await _repoLista.GetAllAsync();
        }

        public async Task InsertBackgroundAsync(int recordId, List<ListaAntecedentes> insertList, List<ListaAntecedentes> deleteList)
        {
            if (deleteList.Count > 0) 
            {
                foreach (ListaAntecedentes background in deleteList)
                {
                    await _repo.DeleteByIdsAsync(recordId,background.Id);
                }
            }

            if (insertList.Count > 0)
            {
                
                foreach (ListaAntecedentes background in insertList)
                {
                    Antecedentes _bg = new Antecedentes()
                    {
                        IdExpediente = recordId,
                        IdListaAntecedentes = background.Id,
                        FechaCreacion = DateTime.Now

                    };
                    await _repo.InsertAsync(_bg);
                }

            }
        }

        

    }
}