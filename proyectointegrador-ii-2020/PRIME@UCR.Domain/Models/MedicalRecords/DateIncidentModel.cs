using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class DateIncidentModel
    {
        public Cita date { get; set; }

        public Incidente incident { get; set; }

        public bool appointment_status { get; set; } = true; 

        public CitaMedica appointment { get; set; }

        public CentroMedico medical_center { get; set; }

        //public string type { get; set; } = "incidente";

        //public string type2 { get; set; } = "cita médica"; 

        public string get_appointment_type() {
            if (appointment_status == true) //es una cita medica
            {
                return "cita médica"; 
            }
            else { //es un incidente
                return "incidente"; 
            }
        }

    }
}
