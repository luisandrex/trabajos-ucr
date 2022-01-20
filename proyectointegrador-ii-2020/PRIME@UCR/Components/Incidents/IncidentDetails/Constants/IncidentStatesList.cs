using PRIME_UCR.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Constants
{
    public class IncidentStatesList
    {
        public List<Tuple<string, string>> List { get; set; }

        public IncidentStatesList()
        {
            List = new List<Tuple<string, string>> {
                Tuple.Create(IncidentStates.InCreationProcess.Nombre ,"Iniciado"),
                Tuple.Create(IncidentStates.Created.Nombre,"Creado"),
                Tuple.Create(IncidentStates.Rejected.Nombre, "Rechazado"),
                Tuple.Create(IncidentStates.Approved.Nombre, "Aprobado"),
                Tuple.Create(IncidentStates.Assigned.Nombre, "Asignado"),
                Tuple.Create(IncidentStates.Preparing.Nombre, "Preparación"),
                Tuple.Create(IncidentStates.InOriginRoute.Nombre, "Hacia origen"),
                Tuple.Create(IncidentStates.PatientInOrigin.Nombre, "Colecta"),
                Tuple.Create(IncidentStates.InRoute.Nombre, "Traslado"),
                Tuple.Create(IncidentStates.Delivered.Nombre, "Entregado"),
                Tuple.Create(IncidentStates.Reactivated.Nombre, "Reactivación"),
                Tuple.Create(IncidentStates.Done.Nombre, "Finalizado")
            };
        }
    }
}
