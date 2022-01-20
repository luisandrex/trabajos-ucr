using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.CheckLists
{
    public class CheckList
    {
        public CheckList()
        {
            Items = new List<Item>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public int? Orden { get; set; }
        public string ImagenDescriptiva { get; set; }

        public bool Editable { get; set; }
        public bool Activada { get; set; }
        // List of items in this checklist
        public List<Item> Items { get; set; }
        public TipoListaChequeo MyType { get; set; }
    }
}
