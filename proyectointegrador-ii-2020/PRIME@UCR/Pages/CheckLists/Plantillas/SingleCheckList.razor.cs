using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.CheckLists;
using Microsoft.AspNetCore.Components.Forms;
using PRIME_UCR.Domain.Models.CheckLists;
using System.Linq;
using System;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Web;

namespace PRIME_UCR.Pages.CheckLists.Plantillas
{
    /**
    * This page displays the information of achecklists and its subitems
    * */
    public class SingleCheckListBase : ComponentBase
    {
        public SingleCheckListBase()
        {
            details.Add("");
            details.Add("");
            //mensajes edición
            instruct.Add("Puede editar la lista de chequeo y eliminar, editar y agregar items");
            instruct.Add("No se pueden eliminar ni agregar items porque está siendo utilizada en un incidente");
            //mensajes de activación
            instruct.Add("Puede ser modificada y asignada a incidentes");
            instruct.Add("No puede ser modificada ni asignada a incidentes");
            states.Add("Estado de la plantilla: ");
            states.Add("");
            _statusMessage = "";
        }
        [Parameter]
        public int id { get; set; }

        protected bool createItem { get; set; } = false;
        private bool isDisabled { get; set; } = true;

        protected bool editItem { get; set; } = false;
        protected bool createSubItem { get; set; } = false;

        protected IEnumerable<CheckList> lists { get; set; } = null;

        protected IEnumerable<Item> coreItems { get; set; } = null;

        protected IEnumerable<Item> items { get; set; } = null;
        protected IEnumerable<Item> subItems { get; set; } = null;
        protected List<Item> itemsList = new List<Item>();

        protected List<Item> orderedList = null;
        protected List<int> orderedListLevel = null;
        protected List<string> _types { get; set; }

        public CheckList list { get; set; }
        public CheckList editedList { get; set; }

        protected Item tempItem;
        protected int parentItemId { get; set; }

        public int startingIndex = -1;
        public int? invalidItemMoved = -1;
        public string _moveItemInvalid = "No puede colocar este item en ese lugar.";

        protected bool formInvalid = false;
        protected EditContext editContext;
        // list for template state to edith checklist
        public List<string> states = new List<string>();
        // details for state
        public List<string> details = new List<string>();
        //array of instruction
        public List<string> instruct = new List<string>();
        public string _statusMessage;

        [Inject] protected ICheckListService MyCheckListService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            lists = null;
            coreItems = null;
            items = null;
            subItems = null;
            itemsList = new List<Item>();
            orderedList = null;
            orderedListLevel = null;

            editedList = new CheckList();
            await RefreshModels();
        }

        protected override void OnParametersSet()
        {
            lists = null;
            coreItems = null;
            items = null;
            subItems = null;
            itemsList = new List<Item>();
            orderedList = null;
            orderedListLevel = null;
            invalidItemMoved = -1;

            createItem = false;
            createSubItem = false;
            editItem = false;
        }
        /**
         * Gets the checklist corresponding to this page, its items and the list of checklists in the database
         * */
        protected async Task RefreshModels()
        {
            list = await MyCheckListService.GetById(id);
            lists = await MyCheckListService.GetAll();
            items = await MyCheckListService.GetItemsByCheckListId(id);
            itemsList = items.ToList();
            coreItems = await MyCheckListService.GetCoreItems(id);
            orderedList = new List<Item>();
            orderedListLevel = new List<int>();
            _types = new List<string>();
            IEnumerable<TipoListaChequeo> types = await MyCheckListService.GetTypes();
            foreach (var type in types)
            {
                _types.Add(type.Nombre);
            }
            foreach (var item in coreItems)
            {
                GenerateOrderedList(item, 0);
            }
            editContext = new EditContext(list);
            editContext.OnFieldChanged += HandleFieldChanged;
            editedList.Nombre = list.Nombre;
            editedList.Descripcion = list.Descripcion;
            editedList.Tipo = list.Tipo;
            editedList.Orden = list.Orden;
            updateState();
        }
        public void updateState()
        {
            if (list.Editable)
            {
                details[0] = "Editable";
                details[1] = instruct[0];
            }
            else
            {
                isDisabled = true;
                createItem = false;
                editItem = false;
                createSubItem = false;
                details[0] = "No editable";
                details[1] = instruct[1];
            }
        }
        protected async Task UpdateActive()
        {
            _statusMessage = "";
            if (list.Activada == true)
            {
                list.Activada = false;
            }
            else
            {
                list.Activada = true;
                isDisabled = true;
                createItem = false;
                editItem = false;
                createSubItem = false;
            }
            await UpdateActivation();

        }

        protected void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            formInvalid = editContext.Validate();
            if (formInvalid == true)
            {
                StateHasChanged();
            }
        }

        /**
         * Refreshes de page models and flags when an item creation is completed
         * */
        protected async Task creatingFinished()
        {
            createItem = false;
            createSubItem = false;
            editItem = false;
            formInvalid = false;
            _statusMessage = "Se guardaron los cambios exitosamente.";
            await RefreshModels();
            StateHasChanged();
        }

        protected async Task editingFinished()
        {
            createItem = false;
            createSubItem = false;
            editItem = false;
            formInvalid = false;
            _statusMessage = "Se guardaron los cambios exitosamente.";
            await RefreshModels();
            StateHasChanged();
        }

        public void Dispose()
        {
            _statusMessage = "";

            createItem = false;
            createSubItem = false;
            editItem = false;
            formInvalid = false;
            StateHasChanged();
        }

        /**
         * Sets flags to display the item creation form
         * */
        protected void StartNewItemCreation()
        {
            _statusMessage = "";
            tempItem = new Item();
            tempItem.IDSuperItem = null;
            tempItem.IDLista = id;
            tempItem.Orden = coreItems.Count() + 1;
            editItem = false;
            createSubItem = false;
            createItem = true;
        }

        /**
         * Sets flags to display the sub item creation form
         * */
        protected async Task CreateSubItem(int itemId)
        {
            _statusMessage = "";
            subItems = await MyCheckListService.GetItemsBySuperitemId(itemId);
            tempItem = new Item();
            tempItem.IDLista = id;
            tempItem.IDSuperItem = itemId;
            tempItem.Orden = subItems.Count() + 1;
            parentItemId = itemId;
            createItem = false;
            editItem = false;
            createSubItem = true;
        }

        protected async Task EditItem(int itemId)
        {
            _statusMessage = "";
            tempItem = await MyCheckListService.GetItemById(itemId);
            parentItemId = itemId;
            createItem = false;
            createSubItem = false;
            editItem = true;
        }

        protected async Task DeleteItem(int itemId)
        {
            _statusMessage = "";
            await MyCheckListService.DeleteItems(itemId);
            await RefreshModels();
        }

        

        protected override async Task OnParametersSetAsync()
        {
            await RefreshModels();
        }

        protected async Task UpdateCheckList()
        {
            _statusMessage = "";
            list.Nombre = editedList.Nombre;
            list.Descripcion = editedList.Descripcion;
            list.Tipo = editedList.Tipo;
            list.Orden = editedList.Orden;
            await MyCheckListService.UpdateCheckList(list);
            _statusMessage = "Se guardaron los cambios exitosamente.";
            await RefreshModels();
            formInvalid = false;
        }
        protected async Task UpdateActivation()
        {
            await MyCheckListService.UpdateCheckList(list);
            _statusMessage = "Se guardaron los cambios exitosamente.";
            await RefreshModels();
        }

        /**
         * Gets an item based on its id
         * */
        protected int getItemIndex(Item itemInList, List<Item> list)
        {
            return list.FindIndex(item => item.Id == itemInList.Id);
        }

        /**
         * Generates an ordered list of an item's subitems based on its level
         * */
        private void GenerateOrderedList(Item item, int level)
        {
            orderedList.Add(item);
            orderedListLevel.Add(level);
            item.SubItems = item.SubItems.OrderBy(item => item.Orden).ToList<Item>();
            List<Item> subItems = itemsList.FindAll(tempItem => tempItem.IDSuperItem == item.Id);
            subItems = subItems.OrderBy(item => item.Orden).ToList<Item>();
            if (subItems.Count() > 0)
            {
                foreach (var tempSubtem in subItems)
                {
                    GenerateOrderedList(tempSubtem, level + 1);
                }
            }
        }

        /**
         * Checks if an item has subitems
         * */
        protected bool HasSubItems(Item item)
        {
            List<Item> subItems = itemsList.FindAll(tempItem => tempItem.IDSuperItem == item.Id);
            bool hasSubItems = false;
            if (subItems.Count() != 0)
            {
                hasSubItems = true;
            }
            return hasSubItems;
        }

        protected string truncate(string text, int level, int lines)
        {
            if (String.IsNullOrEmpty(text)) return "";
            int maxLength = lines * (60 - level * 5);
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }

        protected void StartDrag(Item item)
        {
            startingIndex = orderedList.FindIndex(i => i.Id == item.Id);
        }

        // Updates the order of the items
        protected async Task ReorderItems(Item item, int oldIndex, int newIndex)
        {
            _statusMessage = "";
            int startingIndexReorder = -1;
            int endingIndexReorder = -1;
            int shiftOrder = 1;

            // Checks if the item was placed above or bellow its origial position
            if (newIndex + 1 < orderedList.Count() && orderedList[newIndex + 1].Orden < item.Orden)
            {
                startingIndexReorder = newIndex + 1;
                endingIndexReorder = oldIndex;
                item.Orden = orderedList[newIndex + 1].Orden;
            }
            else if (newIndex - 1 >= 0 && orderedList[newIndex - 1].Orden > item.Orden)
            {
                shiftOrder = -1;
                endingIndexReorder = newIndex - 1;
                startingIndexReorder = oldIndex;
                item.Orden = orderedList[newIndex - 1].Orden;
            }

            await MyCheckListService.UpdateItem(item);

            Item TempItem;
            for (int index = startingIndexReorder; 0 <= index && index < orderedList.Count() && index <= endingIndexReorder; ++index)
            {
                TempItem = orderedList[index];
                if (TempItem.IDSuperItem == item.IDSuperItem)
                {
                    TempItem.Orden += shiftOrder;
                    await MyCheckListService.UpdateItem(TempItem);
                }
            }
            _statusMessage = "Se guardaron los cambios exitosamente.";
            await RefreshModels();
            StateHasChanged();
        }

        protected bool CanDragToParent(Item item, Item ItemUnder)
        {
            bool canDrop = false;
            if (ItemUnder == null)
            {
                canDrop = true;
            }
            else
            {
                canDrop = item.IDSuperItem == ItemUnder.IDSuperItem;
            }
            invalidItemMoved = canDrop ? -1 : item.Id;
            return canDrop;
        }

        protected async void Drop(Item item)
        {
            if (item != null && startingIndex >= 0 && startingIndex < orderedList.Count())
            {
                int endingIndex = getItemIndex(item, orderedList);
                if (endingIndex == startingIndex) return; // Item is in the same position

                // check for parent
                Item draggedItem = orderedList[startingIndex];

                if (!CanDragToParent(draggedItem, item)) return;

                int level = orderedListLevel[startingIndex];

                orderedList.RemoveAt(startingIndex);
                orderedList.Insert(endingIndex, draggedItem);

                orderedListLevel.RemoveAt(startingIndex);
                orderedListLevel.Insert(endingIndex, level);

                await ReorderItems(draggedItem, startingIndex, endingIndex);
                StateHasChanged();
            }
        }
    }
}