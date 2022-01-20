using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table NúmeroTeléfono from database.
     */
    public class NúmeroTeléfono
    {
        /*Foreign key to indicate the identifier of the person with the phone number*/
        public string CedPersona { get; set; }

        /*Object to store the information of the person to which belongs the phone number*/
        public Persona Persona { get; set; }

        /*String to save the phone number of the person*/
        public string NúmeroTelefónico { get; set; }
    }
}