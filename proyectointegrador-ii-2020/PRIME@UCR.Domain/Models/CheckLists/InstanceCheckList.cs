using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.CheckLists
{
    public class InstanceChecklist
    {
        public InstanceChecklist()
        {
            //InstanceItems = new List<InstanceItem>();
        }
        public int PlantillaId { get; set; }
        public string IncidentCod { get; set; }
        public bool Completado { get; set; }
        public DateTime? FechaHoraInicio { get; set; }
        public DateTime? FechaHoraFinal { get; set; }

        // List of items in this checklist
        // public List<InstanceItem> InstanceItems { get; set; }

        //if checklist is complete or not
        public bool IsCompleted()
        {
            //¿recorrer items? to review
            return Completado;
        }

       /* public bool IsModifiable(Estado currentState)
        {
            //verificar estado del indidente
            return currentState.Nombre == IncidentStates.InCreationProcess.Nombre ||
                   currentState.Nombre == IncidentStates.Created.Nombre;
        }*/
    }
}