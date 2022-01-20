using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Incidents.Mappers
{
    public sealed class IncidentListModelMap : ClassMap<IncidentListModel> 
    {  
        public IncidentListModelMap()
        {
            Map(x => x.Codigo).Name("Código");
            Map(x => x.Origen).Name("Origen"); 
            Map(x => x.Destino).Name("Destino"); 
            Map(x => x.Modalidad).Name("Modalidad");
            Map(x => x.FechaHoraRegistro).Name("Fecha y hora de registro");
            Map(x => x.Estado).Name("Estado");
            Map(x => x.IdDestino).Name("Id Destino");

        }

    }
}


/*Codigo = incident.Codigo,
                    Origen = incident.Origen.ToString(),
                    Destino = incident.Destino.ToString(),
                    Modalidad = incident.Modalidad,
                    FechaHoraRegistro = incident.Cita.FechaHoraEstimada,
                    Estado = incident.EstadoIncidentes.ToArray()[incident.EstadoIncidentes.Count - 1].NombreEstado,
                    IdDestino = incident.IdDestino*/