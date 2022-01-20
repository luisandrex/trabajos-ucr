using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table TienePerfil from database.
     */
    public class TienePerfil
    {
        /*Identifier of the corresponding PRIME worker*/
        public string CedFuncionario { get; set; }
        /*Object used to store the PRIME worker itself*/
        public Funcionario Funcionario { get; set; }

        /*Identifier of the profile that the PRIME worker has*/
        public string IDPerfil { get; set; }
        /*Object used to store the profile itself*/
        public Perfil Perfil { get; set; }
    }
}
