using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;

namespace PRIME_UCR.Components.MedicalRecords
{
    public partial class PaginationDropdown
    {

        [Parameter] public EventCallback<int> changeSize { get; set; }

        [Parameter] public int MaxElements { get; set; }
        public List<PageSize> pagesizes { get; set; }

        public int index = 3; 

        private async Task ChangePageSize(int size) {
            await changeSize.InvokeAsync(size); 
        }

        protected override void OnParametersSet()
        {

            base.OnParametersSet();
        }

    }
}


/*
 
        protected override Task OnInitializedAsync()
        {

            pagesizes = new List<PageSize>() {

                new PageSize{ size = "3", idSize = "3"},
                new PageSize { size = "5", idSize = "5"},
                new PageSize { size = "7", idSize = "7"},
                new PageSize {size = "9", idSize = "9"},
            };

            return base.OnInitializedAsync();
        } 
 
 */