using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table Permite from database.
     */
    public class Permite
    {
        /*Identifier of the profile that contains certain permission*/
        public string IDPerfil { get; set; }
        /*Object used to store the profile itself*/
        public Perfil Perfil { get; set; }

        /*Identifier of the permission the profile has access to*/
        public int IDPermiso { get; set; }
        /*Object used to store the permission itself*/
        public Permiso Permiso { get; set; }
    }
}
