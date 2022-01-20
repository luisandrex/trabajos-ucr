using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models
{
    public class TrabajaEn
    {
        public string CédulaMédico { get; set; }
        public int CentroMedicoId { get; set; }
        public Médico Médico { get; set; }
        public CentroMedico CentroMedico { get; set; }

    }
}
