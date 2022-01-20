using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table Usuario from database.
     */
    public class Usuario : IdentityUser 
    {
        /**
        * Function:        Initialize each of the list.
        */
        public Usuario()
        {
            UsuariosYPerfiles = new List<Pertenece>();
        }

        /*Identifier of the person that has the user*/
        public string CedPersona { get; set; }

        /*Object with the information of the person*/
        public Persona Persona { get; set; }

        /*List of profiles of the person*/
        public List<Pertenece> UsuariosYPerfiles { get; set; }
    }
}
