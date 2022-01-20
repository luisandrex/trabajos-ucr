using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;

namespace PRIME_UCR.Application.Repositories.CheckLists
{
    public interface IMultimediaContentItemRepository
    {
        Task<MultimediaContentItem> InsertAsync(MultimediaContentItem mcItem);
    
        Task <IEnumerable<MultimediaContent>> GetByCheckListItem(int itemId, int listId, string incidentCode);

    
    }
}
