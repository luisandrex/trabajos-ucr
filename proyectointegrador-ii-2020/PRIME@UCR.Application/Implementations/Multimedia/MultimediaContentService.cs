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
using PRIME_UCR.Domain.Models.CheckLists;
using PRIME_UCR.Application.Repositories.CheckLists;

namespace PRIME_UCR.Application.Implementations.Multimedia
{
    public class MultimediaContentService : IMultimediaContentService
    {
        //public IFileListEntry file;
        private readonly IMultimediaContentRepository mcRepository;
        private readonly IActionRepository actionRepository;
        private readonly IMultimediaContentItemRepository mcItemRepository;

        public MultimediaContentService(IMultimediaContentRepository mcRepository, IActionRepository actionRepository, IMultimediaContentItemRepository mcItemRepository)
        {
            this.mcRepository = mcRepository;
            this.actionRepository = actionRepository;
            this.mcItemRepository = mcItemRepository;
        }

        public async Task<Accion> AddMultContToAction(int citaId, string nombreAccion, int mcId)
        {
            Accion accion = new Accion
            {
                CitaId = citaId,
                NombreAccion = nombreAccion,
                MultContId = mcId
            };
            accion = await actionRepository.InsertAsync(accion);
            return accion;
        }

        public async Task<IEnumerable<MultimediaContent>> GetByAppointmentAction(int citaId, string nombreAccion)
        {
            return await actionRepository.GetByAppointmentAction(citaId, nombreAccion);
        }

        public async Task<MultimediaContent> AddMultimediaContent(MultimediaContent mcontent) {

            return await mcRepository.InsertAsync(mcontent);
        }
        public async Task<MultimediaContent> GetById(int id)
        {
            return await mcRepository.GetByKeyAsync(id);
        }

        public async Task<MultimediaContentItem> AddMultContToCheckListItem(MultimediaContentItem mcItem)
        {
            return await mcItemRepository.InsertAsync(mcItem);
        }

        public async Task<IEnumerable<MultimediaContent>> GetByCheckListItem(int itemId, int listId, string incidentCode)
        {
            return await mcItemRepository.GetByCheckListItem(itemId, listId, incidentCode);
        }
        public async Task DeleteMultimediaContent(MultimediaContent mcontent)
        {
            await mcRepository.DeleteAsync(mcontent.Id);
        }

    }
}