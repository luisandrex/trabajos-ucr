using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Dashboard
{
    public class IncidentToFilterModel
    {
        public Distrito District { get; set; }
        public Canton Canton { get; set; }
        public Provincia Province { get; set; }
        public Incidente Incidente { get; set; }
    }
}
