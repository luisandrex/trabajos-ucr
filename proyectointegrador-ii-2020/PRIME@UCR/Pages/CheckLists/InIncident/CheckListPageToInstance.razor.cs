using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Services;
using PRIME_UCR.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Components.CheckLists.InIncident;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using PRIME_UCR.Domain.Models.CheckLists;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Pages.CheckLists.InIncident
{
    public class CheckListToInstanceBase : ComponentBase
    {
        
        public CheckListToInstanceBase()
        {
            llenado = false;
            count = 0;
            dont_save = true;
        }
        protected IEnumerable<CheckList> lists { get; set; }
        public bool llenado;
        protected bool _exist = true;
        [Inject] protected ICheckListService MyService { get; set; }
        [Inject] protected IIncidentService IncidentService { get; set; }
        public List<CheckList> TempInstance = new List<CheckList>();
        public List<Todo> TempDetail = new List<Todo>();
        public List<int> TempsIds = new List<int>();
        private IncidentDetailsModel _incidentModel;
        public int count;
        public bool dont_save;
        [Parameter] public string incidentcod { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _incidentModel = await IncidentService.GetIncidentDetailsAsync(incidentcod);
            if (_incidentModel == null)
                _exist = false;
            else
                await RefreshModels();
        }

        protected async Task RefreshModels()
        {
            lists = await MyService.GetAll();
        }
        protected async Task Update()
        {
            await MyService.UpdateCheckList((CheckList)lists);
            await RefreshModels();
        }
        public void Dispose()
        {
            foreach (var temp in TempDetail)
            {
                temp.IsDone = false;
            }
            Update();
            OnInitialized();
            dont_save = true;
        }
        /*protected bool CheckIempList(int id)
        {
            bool resultado = false;
            if (TempIds.Contains(id))
            {
                TempIds.Remove(id);
                resultado = true;
            }
            else
            {
                TempIds.Add(id);
                resultado = false;
            }
            //TempInstanceIds.Append();
            RefreshModels();
            StateHasChanged();
            return resultado;
        }*/
        protected void CheckIempList(int idd, ChangeEventArgs e)
        {
            if ((bool)e.Value)
            {
                TempDetail[TempsIds.IndexOf(idd)].IsDone = true;
                count++;
                update_save();
            }
            else
            {
                TempDetail[TempsIds.IndexOf(idd)].IsDone = false;
                count--;
                update_save();
            }
        }

        public void update_save()
        {
            if (count > 0)
            {
                dont_save = false;
            }
            else
            {
                dont_save = true;
            }
        }

        protected void toggleItemChangeComponent()
        {
        }
        public class Todo
        {
            public bool IsDone { get; set; }
            public int idd;
        }

        public void Llenar() {
             foreach (var templist in lists)
             {
                 Todo TodoItem =new Todo();
                 int idds = @templist.Id;
                 TodoItem.idd = idds;
                 TempDetail.Add(TodoItem);
                TempsIds.Add(idds);
             }
            llenado = true;
        }
    }
}