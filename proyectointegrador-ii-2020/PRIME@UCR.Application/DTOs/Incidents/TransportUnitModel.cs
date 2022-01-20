using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Incidents
{
    public class TransportUnitModel
    {
        public string Matricula { get; set; }
        public string Estado { get; set; }
        public string Modalidad { get; set; }
        public Modalidad ModalidadTransporte { get; set; }

    }
}
