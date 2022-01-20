using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    /**
     * Class used to model table Funcionario from database.
     */
    public class Funcionario : Persona
    {
        public Funcionario()
        {
            PerfilesDelFuncionario = new List<TienePerfil>();
        }
        /*List of profiles for which the Funcionario is part*/
        public List<TienePerfil> PerfilesDelFuncionario { get; set; }
    }
}
