using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Dashboard
{
    public class IncidentsCounterModel
    {
        public IncidentsCounterModel()
        {
            isReadyToShowCounters = false;
        }

        public int totalIncidentsCounter;

        public int groundIncidentsCounter;

        public int airIncidentsCounter;

        public int maritimeIncidents;

        public bool isReadyToShowCounters;
    }
}
