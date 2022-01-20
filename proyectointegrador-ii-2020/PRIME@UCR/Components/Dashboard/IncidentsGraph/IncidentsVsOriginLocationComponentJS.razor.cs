using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Dashboard.IncidentsGraph
{
    public partial  class IncidentsVsOriginLocationComponentJS
    {
        [Parameter] public FilterModel Value { get; set; }
        [Parameter] public EventCallback<FilterModel> ValueChanged { get; set; }
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }

        [Parameter] public bool ZoomActive { get; set; }

        private int eventQuantity { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GenerateIncidentsVsOriginLocationComponentJS();
        }

        protected override async Task OnParametersSetAsync()
        {
            await GenerateIncidentsVsOriginLocationComponentJS();
        }


        /*
         *  GenerateIncidentsVsOriginLocationComponentJS()
         *  
         *  Method that generates the IncidentsVsOrigin Graph 
         *  based on the incoming list from the dashboard service in which 
         *  data is already filtered 
         *  
         */
        private async Task GenerateIncidentsVsOriginLocationComponentJS()
        {
            var incidentsData = Data.filteredIncidentsData;
            var medicalCenters = Data.medicalCenters;
            var districtsData = Data.districts;

            var AllIncidentsData = Data.incidentsData;

            eventQuantity = incidentsData.Count();

            var incidentsPerOrigin = new List<IGrouping<string, Incidente>>();
            
            if (Value.OriginType == "Internacional")
            {
                incidentsPerOrigin = incidentsData.GroupBy(i => {
                    if (i.Origen != null)
                    {
                        var inter = i.Origen as Internacional;
                        return inter.NombrePais;
                    }
                    else
                    {
                        return null;
                    }
                }).ToList();
            } else if (Value.OriginType == "Centro médico")
            {
                incidentsPerOrigin = incidentsData.GroupBy(i => {
                    if (i.Origen != null)
                    {
                        var centroUbicacion = i.Origen as CentroUbicacion;
                        return medicalCenters.FirstOrDefault(mc => mc.Id == centroUbicacion.CentroMedicoId)?.Nombre;
                    }
                    else
                    {
                        return null;
                    }
                }).ToList();
            } else if (Value.OriginType == "Domicilio")
            {
                if(Value.HouseholdOriginFilter.District != null)
                {
                    incidentsPerOrigin = incidentsData.GroupBy(i => {
                        if (i.Origen != null)
                        {
                            var domicilio = i.Origen as Domicilio;
                            return districtsData.Find(district => domicilio?.DistritoId == district.Id)?.Nombre;
                        }
                        else
                        {
                            return null;
                        }
                    }).ToList();
                } else if (Value.HouseholdOriginFilter.Canton != null)
                {
                    incidentsPerOrigin = incidentsData.GroupBy(i => {
                        if (i.Origen != null)
                        {
                            var domicilio = i.Origen as Domicilio;
                            return districtsData.Find(district => domicilio?.DistritoId == district.Id)?.Nombre;
                        }
                        else
                        {
                            return null;
                        }
                    }).ToList();
                } else if (Value.HouseholdOriginFilter.Province != null)
                {
                    incidentsPerOrigin = incidentsData.GroupBy(i => {
                        if (i.Origen != null)
                        {
                            var domicilio = i.Origen as Domicilio;
                            return districtsData.Find(district => domicilio?.DistritoId == district.Id)?.Canton.Nombre;
                        }
                        else
                        {
                            return null;
                        }
                    }).ToList();
                } else
                {
                    incidentsPerOrigin = incidentsData.GroupBy(i => {
                        if (i.Origen != null)
                        {
                            var domicilio = i.Origen as Domicilio;
                            return districtsData.Find(district => domicilio?.DistritoId == district.Id)?.Canton.NombreProvincia;
                        }
                        else
                        {
                            return null;
                        }
                    }).ToList();
                }
            } else
            {
                incidentsPerOrigin = incidentsData.GroupBy(i => {
                    if(i.Origen != null)
                    {
                        return i.Origen?.GetType().Name;    
                    } else
                    {
                        return null;
                    }
                }).ToList();
            }


            var results = new List<String>();


            foreach (var incidents in incidentsPerOrigin)
            {
                var labelName = "No Asignado";
                if(incidents.Key != null)
                {
                    labelName = incidents.Key;
                }
                results.Add(labelName);
                results.Add(incidents.ToList().Count().ToString());
            }

            await JS.InvokeVoidAsync("CreateIncidentsVsOriginLocationComponentJS", (object)results);
        }

        void ShowModal()
        {
            var modalOptions = new ModalOptions()
            {
                Class = "graph-zoom-modal blazored-modal"
            };

            var parameters = new ModalParameters();
            parameters.Add(nameof(IncidentsVsOriginLocationComponentJS.Value), Value);
            parameters.Add(nameof(IncidentsVsOriginLocationComponentJS.Data), Data);
            parameters.Add(nameof(IncidentsVsOriginLocationComponentJS.ZoomActive), true);
            Modal.Show<IncidentsVsOriginLocationComponentJS>("Incidentes por origen", parameters, modalOptions);
        }
    }
}
