using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table Permiso from database.
     */
    public class Permiso
    {
        /**
         * Function:        Initialize each of the list.
         */
        public Permiso()
        {
            PerfilesYPermisos = new List<Permite>();
        }

        /*Identifier of the permission*/
        public int IDPermiso { get; set; }

        /*Description of the permission*/
        public string DescripciónPermiso { get; set; }

        /*List of the profile that shares the permission*/
        public List<Permite> PerfilesYPermisos { get; set; }
    }
}
