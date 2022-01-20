using PRIME_UCR.Domain.Models.CheckLists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.CheckLists
{
    public interface IInstanceItemRepository : IGenericRepository<InstanciaItem, int>
    {
        Task<IEnumerable<InstanciaItem>> GetByIncidentCodAndCheckListId(string incidentCode, int checklistId);
        Task<IEnumerable<InstanciaItem>> GetCoreItems(string incidentCode, int checklistId);
        Task<IEnumerable<InstanciaItem>> GetItemsByFatherId(string incidentCode, int checklistId, int itemId);
        Task<InstanciaItem> GetItem(int? itemId, string incidentCod, int? plantillaId);
    }
}
