using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.DTOs.Incidents
{
    public class AssignmentModel
    {
        public AssignmentModel()
        {
            TeamMembers = new List<EspecialistaTécnicoMédico>();
        }

        public UnidadDeTransporte TransportUnit { get; set; }
        public CoordinadorTécnicoMédico Coordinator { get; set; }
        public List<EspecialistaTécnicoMédico> TeamMembers { get; set; }
    }
}
