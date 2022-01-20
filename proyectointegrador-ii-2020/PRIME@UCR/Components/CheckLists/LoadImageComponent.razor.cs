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
using System;

namespace PRIME_UCR.Components.CheckLists
{
    /**
    * This component allows the user to upload an image into a checklist or an item
    * */
    public class LoadImageComponentBase : ComponentBase
    {
        [Inject] public IFileService file_service { get; set; }

        [Inject] public ICheckListService checklist_service { get; set; }

        /**
        * Shares the data from a checklist with its parent component
        * */
        [Parameter]
        public EventCallback<CheckList> listChanged { get; set; }
        [Parameter]
        public CheckList list { get; set; }

        /**
        * Shares the data from an item with its parent component
        * */
        [Parameter]
        public EventCallback<Item> itemChanged { get; set; }
        [Parameter]
        public Item item { get; set; }

        /**
        * Determines whether or not an already existing entry in the dabase should be updated
        * */
        [Parameter]
        public string update { get; set; }

        protected string lastFile = "";
        protected string dropClass = "";
        protected string[] acceptedTypes = { "image/png", "image/gif", "image/jpeg", "image/jpg", "image/svg" };

        /**
        * Sets the class of the dropdown zone to upload an image
        * */
        protected void HandleDragEnter()
        {
            dropClass = "dropzone-drag";
        }

        /**
        * Clears the class of the dropdown zone to upload an image
        * */
        protected void HandleDragLeave()
        {
            dropClass = "";
        }

        /**
        * Updates the data from the checklist to its parent component
        * */
        protected Task OnlistChanged()
        {
            return listChanged.InvokeAsync(list);
        }

        /**
        * Updates the data from the item to its parent component
        * */
        protected Task OnitemChanged()
        {
            return itemChanged.InvokeAsync(item);
        }

        /**
        * Saves the image uploaded by the user
        * */
        protected async Task HandleSelectedImage(IFileListEntry[] files)
        {
            dropClass = "";
            IFileListEntry file = files.FirstOrDefault();

            if (!acceptedTypes.Contains(file.Type)) return;

            string filePath = "/datas/" + file.Name;
            // Update the name of the uploaded image
            lastFile = file.Name;

            // stores the file (without encrypting it) in the /wwwroot/images directory)
            await file_service.StoreFileNoEncryption(file.Name, file.Data);

            if (list != null)
            {
                // Saves the name of the file into the correconding checklist entry
                list.ImagenDescriptiva = filePath;
                await OnlistChanged();
                if (update == "update")
                {
                    await checklist_service.UpdateCheckList(list);
                }
            }
            else if (item != null)
            {
                // Saves the name of the file into the correconding item entry
                item.ImagenDescriptiva = filePath;
                await OnitemChanged();
                if (update == "update")
                {
                    await checklist_service.UpdateItem(item);
                }
            }
        }
    }
}