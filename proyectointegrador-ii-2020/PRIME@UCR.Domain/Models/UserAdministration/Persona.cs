using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table Persona from database.
     */
    public class Persona
    {
        /**
         * Function:        Initialize each of the list.
         */

        /*String that identify the person*/
        public string NombreCompleto => $"{Nombre} {PrimerApellido} {SegundoApellido}";

        public string Cédula { get; set; }

        /*String that store the name of the person*/
        public string Nombre { get; set; }

        /*String that store the first last name of the person*/
        public string PrimerApellido { get; set; }

        /*String that store the second last name of the person*/
        public string? SegundoApellido { get; set; }

        /*Character that store the sex of the person*/
        public string? Sexo { get; set; }

        /*Variable that store the birth date of the person*/
        public DateTime? FechaNacimiento { get; set; }

    }
}
