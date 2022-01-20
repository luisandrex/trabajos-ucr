using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table Pertenece from database.
     */
    public class Pertenece
    {
        /*Identifier of the user which has a certain profile assigned to it*/
        public string IDUsuario { get; set; }
        /*Object used to store the user itself*/
        public Usuario Usuario { get; set; }
        /*Identifier of the profile that the user has*/
        public string IDPerfil { get; set; }
        /*Object used to store the profile itself*/
        public Perfil Perfil { get; set; }
    }
}
