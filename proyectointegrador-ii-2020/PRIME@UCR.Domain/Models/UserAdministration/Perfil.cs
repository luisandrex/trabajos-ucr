using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table Perfil from database.
     */
    public class Perfil
    {
        /**
         * Function:        Initialize each of the list.
         */
        public Perfil()
        {
            PerfilesYPermisos = new List<Permite>();
            UsuariosYPerfiles = new List<Pertenece>();
            FuncionariosYPerfiles = new List<TienePerfil>();
        }

        /*String to store the name of the profile*/
        public string NombrePerfil { get; set; }

        /*List of permissions for the profile*/
        public List<Permite> PerfilesYPermisos { get; set; }

        /*List of users that have the profile*/
        public List<Pertenece> UsuariosYPerfiles { get; set; }

        /*List of functionaries that have the profile*/
        public List<TienePerfil> FuncionariosYPerfiles { get; set; }
    }
}
