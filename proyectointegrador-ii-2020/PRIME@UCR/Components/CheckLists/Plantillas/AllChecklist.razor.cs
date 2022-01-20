using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Services;
using PRIME_UCR.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Components.CheckLists.Plantillas;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using PRIME_UCR.Domain.Models.CheckLists;
using MatBlazor;
using BlazorTable;

namespace PRIME_UCR.Components.CheckLists.Plantillas
{
    /**
    * This page displays every checklist instance in especific incident and general data for each one
    * */
    public class AllChecklistBase : ComponentBase
    {

        [Inject]
        private NavigationManager NavManager { get; set; }

        protected IEnumerable<CheckList> lists { get; set; }
        public  ITable<CheckList> Table;
        [Inject] protected ICheckListService MyCheckListService { get; set; }
        public CheckList Alist = new CheckList(); //templist

        protected override async Task OnInitializedAsync()
        {
            await RefreshModels();
        }

        /**
         * Gets the list of checklists in the database
         * */
        protected async Task RefreshModels()
        {
            lists = await MyCheckListService.GetAll();
        }

    }
}
