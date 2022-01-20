using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Application.Services.Multimedia;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BlazorInputFile;
using PRIME_UCR.Domain.Models.CheckLists;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace PRIME_UCR.Components.CheckLists.Plantillas
{
    /**
    * This component allows the user to upload an image into a checklist or an item
    * */
    public class EditItemBase : ComponentBase
    {
        [Inject] protected ICheckListService MyService { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected bool formInvalid = false;
        protected EditContext editContext;

        /**
        * Shares the data from an item with its parent component
        * */
        [Parameter]
        public EventCallback<Item> itemChanged { get; set; }
        [Parameter]
        public Item item { get; set; }

        /**
        * Shares the data from an item with its parent component
        * */
        [Parameter]
        public EventCallback<int> OnEditingChanged { get; set; }

        [Parameter]
        public bool editing { get; set; }

        protected override void OnInitialized()
        {
            editContext = new EditContext(item);
            editContext.OnFieldChanged += HandleFieldChanged;
            StateHasChanged();
        }

        /**
        * Updates the data from the item to its parent component
        * */
        protected Task OnitemChanged()
        {
            return itemChanged.InvokeAsync(item);
        }

        /**
         * Registers the new item
         * */
        protected async Task EditItem(Item item)
        {
            if (item.ImagenDescriptiva == null)
            {
                item.ImagenDescriptiva = "/images/defaultCheckList.svg";
            }
            formInvalid = false;
            await MyService.UpdateItem(item);
            await OnitemChanged();
            await OnEditingChanged.InvokeAsync(0);
            StateHasChanged();
        }

        protected void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            formInvalid = editContext.Validate();
            if (formInvalid == true)
            {
                StateHasChanged();
            }
        }

        public async void Dispose()
        {
            editing = false;
            await OnEditingChanged.InvokeAsync(0);
            editContext.OnFieldChanged -= HandleFieldChanged;
            StateHasChanged();

        }

        protected async Task HandleValidSubmit()
        {
            await EditItem(item);
        }
    }
}