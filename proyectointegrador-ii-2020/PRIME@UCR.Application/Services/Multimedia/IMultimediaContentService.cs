using PRIME_UCR.Application.Services;
using PRIME_UCR.Application.Repositories;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.CheckLists;

namespace PRIME_UCR.Application.Services.Multimedia
{

    public interface IMultimediaContentService {

        Task<MultimediaContent> AddMultimediaContent(MultimediaContent mcontent);
        Task<MultimediaContent> GetById(int id);
        Task<Accion> AddMultContToAction(int citaId, string nombreAccion, int mcId);
        Task<IEnumerable<MultimediaContent>> GetByAppointmentAction(int citaId, string nombreAccion);

        Task<MultimediaContentItem> AddMultContToCheckListItem(MultimediaContentItem mcItem);

        Task<IEnumerable<MultimediaContent>> GetByCheckListItem(int itemId, int listId, string incidentCode);

        Task DeleteMultimediaContent(MultimediaContent mcontent);

    }
}
