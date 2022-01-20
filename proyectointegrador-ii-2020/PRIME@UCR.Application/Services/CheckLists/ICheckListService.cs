using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.CheckLists
{
    public interface ICheckListService
    {
        Task<IEnumerable<CheckList>> GetAll();
        Task<IEnumerable<TipoListaChequeo>> GetTypes();
        Task<CheckList> InsertCheckList(CheckList list);
        Task<CheckList> GetById(int id);
        Task<Item> GetItemById(int Id);
        Task<CheckList> UpdateCheckList(CheckList list);
        Task<Item> InsertCheckListItem(Item item);
        Task<IEnumerable<Item>> GetItemsByCheckListId(int checkListId);
        Task<IEnumerable<Item>> GetItemsBySuperitemId(int superItemId);
        Task<IEnumerable<Item>> GetCoreItems(int checkListId);
        Task DeleteItems(int itemId);
        Task<Item> UpdateItem(Item item);
        Task<IEnumerable<CheckList>> GetAllChecklistActivates();
    }
}
