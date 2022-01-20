using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.CheckLists
{
    public class TipoListaChequeo
    {
        public string Nombre { get; set; }
        public List<CheckList> Lists { get; set; }
    }
}
