using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.Dtos.Incidents
{
    public class IncidentModel
    {
        public Modalidad Mode { get; set; }
        public DateTime? EstimatedDateOfTransfer { get; set; }

    }
}