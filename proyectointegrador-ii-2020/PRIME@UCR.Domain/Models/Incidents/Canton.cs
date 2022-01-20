using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models
{
    public class Canton
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreProvincia { get; set; }
        public Provincia Provincia { get; set; }
        public List<Distrito> Distritos { get; private set; }
        
        
        public Canton() {
            Distritos = new List<Distrito>();
        }
        
        protected bool Equals(Canton other)
        {
            return Id == other.Id && Nombre == other.Nombre && NombreProvincia == other.NombreProvincia;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Canton) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, NombreProvincia);
        }
    }
}
