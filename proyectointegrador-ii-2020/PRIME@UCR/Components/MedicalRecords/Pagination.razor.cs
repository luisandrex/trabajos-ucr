using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Domain.Models.MedicalRecords;

namespace PRIME_UCR.Components.MedicalRecords
{
    public partial class Pagination
    {
        [Parameter] public int Current_page { get; set; } = 1;
        [Parameter] public int Total_pages { get; set; }
        [Parameter] public int Radius { get; set; } = 3;
        [Parameter] public EventCallback<int> Selected_page { get; set; }

        List<LinkModel> links;

        protected override void OnParametersSet()
        {
            LoadPages();

            base.OnParametersSet();
        }


        private async Task SelectAnotherPage(LinkModel link_model)
        {

            if (link_model.get_page() != Current_page && link_model.is_it_enabled())
            {
                Current_page = link_model.get_page();

                await Selected_page.InvokeAsync(link_model.get_page());

            }
            else
            {
                return;
            }

        }


        private void LoadPages()
        {
            links = new List<LinkModel>();
            var is_previous_page_link_enabled = Current_page != 1;
            var previous_page = Current_page - 1;
            links.Add(new LinkModel(previous_page, is_previous_page_link_enabled, "Anterior"));


            for (int link_model = 1; link_model < Total_pages; ++link_model)
            {

                if (link_model > Current_page - Radius && link_model < Current_page + Radius)
                {

                    bool is_this_link_enabled = true;

                    LinkModel new_link_model = new LinkModel(link_model, is_this_link_enabled, link_model.ToString());

                    new_link_model.change_active_mode(link_model == Current_page);

                    links.Add(new_link_model);

                }
            }

            var is_next_page_link_enabled = Current_page != Total_pages;
            var next_page = Current_page + 1;
            links.Add(new LinkModel(next_page, is_next_page_link_enabled, "Siguiente"));

        }
    }
}
