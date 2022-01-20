using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.CheckLists
{
    public class Item
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? Orden { get; set; }
        public string ImagenDescriptiva { get; set; }
        public int? IDSuperItem { get; set; }    // fk-Item
        public int? IDLista { get; set; }        // fk-CheckList

        public CheckList Checklist { get; set; }

        // List of subitems in this item
        public List<Item> SubItems { get; set; }
        public Item ItemList { get; set; } // parent item
        public List<InstanciaItem> Instances { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Item i)
                return Id == i.Id &&
                       Nombre == i.Nombre &&
                       IDLista == i.IDLista;

            return false;
        }
        public Item()
        {
            SubItems = new List<Item>();
        }
    }
}
