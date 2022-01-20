using System.Collections.Generic;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Domain.Models
{
    public class CentroUbicacion : Ubicacion
    {
        public int CentroMedicoId { get; set; }
        public int? NumeroCama { get; set; }
        public CentroMedico CentroMedico { get; set; }
        public string CedulaMedico { get; set; }
        public Médico Médico { get; set; }
        public override string DisplayName => "Centro médico";
    }
}
