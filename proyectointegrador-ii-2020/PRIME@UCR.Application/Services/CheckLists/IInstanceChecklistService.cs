using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.CheckLists
{
    public interface IInstanceChecklistService
    {
        Task<IEnumerable<InstanceChecklist>> GetAll();
        Task InsertInstanceChecklist(InstanceChecklist list);
        Task<InstanceChecklist> GetById(int id);
        Task<IEnumerable<InstanceChecklist>> GetByIncidentCod(string cod);
        Task<IEnumerable<InstanceChecklist>> GetByIdAndIncidentCode(int checkListId, string IncidentCode);
        Task<InstanceChecklist> UpdateInstanceChecklist(InstanceChecklist list);
        Task<int> GetNumberOfItems(string incidentCode, int checkListId);
        Task<int> GetNumberOfCompletedItems(string incidentCode, int checkListId);
        Task DeleteInstanceChecklist(int id, string cod);
        Task<InstanciaItem> InsertInstanceItem(InstanciaItem instanceItem);
        Task<IEnumerable<InstanciaItem>> GetItemsByIncidentCodAndCheckListId(string incidentCode, int checklistId);
        Task<IEnumerable<InstanciaItem>> GetCoreItems(string incidentCode, int checklistId);
        Task<IEnumerable<InstanciaItem>> GetItemsByFatherId(string incidentCode, int checklistId, int itemId);
        //Task<Item> InsertInstanceChecklistItem(Item item);
        //Task<IEnumerable<Item>> GetItemsByInstanceChecklistId(int checkListId);
        //Task<IEnumerable<Item>> GetItemsBySuperitemId(int superItemId);
        // Task<IEnumerable<Item>> GetCoreItems(int checkListId);
        //Task<Item> SaveImageItem(Item item);
        Task<InstanciaItem> UpdateItemInstance(InstanciaItem item);
        void LoadRelations(List<InstanciaItem> items);
    }
}
