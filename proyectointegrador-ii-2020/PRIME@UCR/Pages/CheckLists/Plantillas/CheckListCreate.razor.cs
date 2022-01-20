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

namespace PRIME_UCR.Pages.CheckLists.Plantillas
{
    /**
    * This page displays a form to create a new checklist
    * */
    public class CheckListCreateBase : ComponentBase
    {
   
        protected IEnumerable<CheckList> lists { get; set; }

        [Inject] protected ICheckListService MyService { get; set; }

        [Inject]  public NavigationManager NavManager { get; set; }

        private const string CreateUrl = "/checklist/create";
        // private string beforeUrl = "/checklist"; //mejorar diseño de interfaz
        private string afterUrl = "";

        protected CheckList checkList = new CheckList();
        protected List<string> _types = new List<string>();

        protected bool formInvalid = false;
        protected EditContext editContext;

        /**
         * Gets the list of checklists in the database
         * */
        protected async Task RefreshModels()
        {
            lists = await MyService.GetAll();
            IEnumerable<TipoListaChequeo> types = await MyService.GetTypes();
            foreach (var type in types)
            {
                _types.Add(type.Nombre);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            editContext = new EditContext(checkList);
            editContext.OnFieldChanged += HandleFieldChanged;
            await RefreshModels();
            checkList.Orden = lists.Count() + 1;
        }

        /**
         * Checks if the required fields of the checklist are valid
         * */
        protected void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            formInvalid = editContext.Validate();
            if (formInvalid == true) 
            {
                StateHasChanged();
            }
        }


        public void Dispose()
        {
            editContext.OnFieldChanged -= HandleFieldChanged;
        }

        /**
         * Inserts the new checklist into the database
         * */
        protected async Task AddCheckList(CheckList tempList)
        {
            tempList.Activada = true;
            tempList.Editable = true;
            if (tempList.ImagenDescriptiva == null)
            {
                tempList.ImagenDescriptiva = "/images/defaultCheckList.svg";
            }
            await MyService.InsertCheckList(tempList);
            await RefreshModels();
        }

        /**
         * Registers the new checklist and navigates to its page
         * */
        protected async Task HandleValidSubmit()
        {
            await AddCheckList(checkList);
            afterUrl = "/checklist/" + checkList.Id;
            NavManager.NavigateTo(afterUrl); // to do: agregar a pagina anterior
        }

    }
}
