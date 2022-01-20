using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.CheckLists
{
    public class InstanciaItem
    {
        public int ItemId { get; set; }
        public int PlantillaId { get; set; }
        public string IncidentCod { get; set; }
        public int? ItemPadreId { get; set; }
        public int? PlantillaPadreId { get; set; }
        public string? IncidentCodPadre { get; set; }
        public bool Completado { get; set; }
        public DateTime? FechaHoraInicio { get; set; }
        public DateTime? FechaHoraFin { get; set; }
        public List<InstanciaItem> SubItems { get; set; }
        public InstanciaItem MyFather { get; set; }
        public Item MyItem { get; set; }

        public InstanciaItem()
        {
            SubItems = new List<InstanciaItem>();
        }
    }
}
