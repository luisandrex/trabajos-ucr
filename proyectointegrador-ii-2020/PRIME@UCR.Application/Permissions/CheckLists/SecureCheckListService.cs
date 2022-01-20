using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Implementations.CheckLists;
using PRIME_UCR.Application.Services.CheckLists;

namespace PRIME_UCR.Application.Permissions.CheckLists
{
    public class SecureCheckListService : ICheckListService
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly CheckListService checkListService;

        public SecureCheckListService(ICheckListRepository checklistRepository,
                                ICheckListTypeRepository checkListTypeRepository,
                                IItemRepository itemRepository,
                                IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            checkListService = new CheckListService(checklistRepository, checkListTypeRepository, itemRepository);
        }

        public async Task<CheckList> InsertCheckList(CheckList list)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanCreateChecklist });
            return await checkListService.InsertCheckList(list);
        }

        public async Task<CheckList> UpdateCheckList(CheckList list)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanCreateChecklist });
            return await checkListService.UpdateCheckList(list);
        }

        public async Task<Item> InsertCheckListItem(Item item)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanCreateChecklist });
            return await checkListService.InsertCheckListItem(item);
        }

        public async Task<Item> UpdateItem(Item item)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanCreateChecklist });
            return await checkListService.UpdateItem(item);
        }

        public async Task<IEnumerable<CheckList>> GetAll()
        {
            return await checkListService.GetAll();
        }

        public async Task<IEnumerable<TipoListaChequeo>> GetTypes()
        {
            return await checkListService.GetTypes();
        }

        public async Task<CheckList> GetById(int id)
        {
            return await checkListService.GetById(id);
        }

        public async Task<Item> GetItemById(int Id)
        {
            return await checkListService.GetItemById(Id);
        }

        public async Task<IEnumerable<Item>> GetItemsByCheckListId(int checkListId)
        {
            return await checkListService.GetItemsByCheckListId(checkListId);
        }

        public async Task<IEnumerable<Item>> GetItemsBySuperitemId(int superItemId)
        {
            return await checkListService.GetItemsBySuperitemId(superItemId);
        }

        public async Task<IEnumerable<Item>> GetCoreItems(int checkListId)
        {
            return await checkListService.GetCoreItems(checkListId);
        }

        public async Task<IEnumerable<CheckList>> GetAllChecklistActivates()
        {
            return await checkListService.GetAllChecklistActivates();
        }

        public async Task DeleteItems(int itemId)
        {
            await checkListService.DeleteItems(itemId);
        }
    }
}
