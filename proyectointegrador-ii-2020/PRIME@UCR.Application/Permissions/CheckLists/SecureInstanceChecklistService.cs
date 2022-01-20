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
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Implementations.CheckLists;

namespace PRIME_UCR.Application.Permissions.CheckLists
{
    public class SecureInstanceChecklistService : IInstanceChecklistService
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly InstanceChecklistService instanceChecklistService;

        public SecureInstanceChecklistService(IInstanceChecklistRepository instancechecklistRepository,
                                        IInstanceItemRepository instanceItemRepository,
                                        IPrimeSecurityService _primeSecurityService)//, IItemRepository itemRepository)
        {
            primeSecurityService = _primeSecurityService;
            instanceChecklistService = new InstanceChecklistService(instancechecklistRepository, instanceItemRepository);
            //_itemRepository = itemRepository;
        }

        public async Task InsertInstanceChecklist(InstanceChecklist list)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanInstantiateChecklist });
            await instanceChecklistService.InsertInstanceChecklist(list);
        }

        public async Task<InstanceChecklist> UpdateInstanceChecklist(InstanceChecklist list)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanCheckItemsInChecklists });
            return await instanceChecklistService.UpdateInstanceChecklist(list);
        }

        public async Task DeleteInstanceChecklist(int id, string cod)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanManageIncidentChecklists });
            await instanceChecklistService.DeleteInstanceChecklist(id, cod);
        }

        public async Task<InstanciaItem> InsertInstanceItem(InstanciaItem instanceItem)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanInstantiateChecklist });
            return await instanceChecklistService.InsertInstanceItem(instanceItem);
        }

        public async Task<InstanciaItem> UpdateItemInstance(InstanciaItem item)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[] { AuthorizationPermissions.CanManageIncidentChecklists });
            return await instanceChecklistService.UpdateItemInstance(item);
        }

        public async Task<IEnumerable<InstanceChecklist>> GetAll()
        {
            return await instanceChecklistService.GetAll();
        }

        public async Task<InstanceChecklist> GetById(int id)
        {
            return await instanceChecklistService.GetById(id);
        }

        public async Task<IEnumerable<InstanceChecklist>> GetByIncidentCod(string cod)
        {
            return await instanceChecklistService.GetByIncidentCod(cod);
        }

        public async Task<IEnumerable<InstanceChecklist>> GetByIdAndIncidentCode(int checkListId, string IncidentCode)
        {
            return await instanceChecklistService.GetByIdAndIncidentCode(checkListId, IncidentCode);
        }

        public async Task<int> GetNumberOfItems(string incidentCode, int checkListId)
        {
            return await instanceChecklistService.GetNumberOfItems(incidentCode, checkListId);
        }

        public async Task<int> GetNumberOfCompletedItems(string incidentCode, int checkListId)
        {
            return await instanceChecklistService.GetNumberOfCompletedItems(incidentCode, checkListId);
        }

        public async Task<IEnumerable<InstanciaItem>> GetItemsByIncidentCodAndCheckListId(string incidentCode, int checklistId)
        {
            return await instanceChecklistService.GetItemsByIncidentCodAndCheckListId(incidentCode, checklistId);
        }

        public async Task<IEnumerable<InstanciaItem>> GetCoreItems(string incidentCode, int checklistId)
        {
            return await instanceChecklistService.GetCoreItems(incidentCode, checklistId);
        }

        public async Task<IEnumerable<InstanciaItem>> GetItemsByFatherId(string incidentCode, int checklistId, int itemId)
        {
            return await instanceChecklistService.GetItemsByFatherId(incidentCode, checklistId, itemId);
        }

        public void LoadRelations(List<InstanciaItem> items)
        {
            instanceChecklistService.LoadRelations(items);
        }
    }
}