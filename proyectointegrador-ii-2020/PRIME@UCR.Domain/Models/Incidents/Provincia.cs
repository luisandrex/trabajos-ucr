using System;
using System.Collections.Generic;

namespace PRIME_UCR.Domain.Models
{
    public class Provincia
    {

        public string Nombre { get; set; }
        public string NombrePais { get; set; }
        public Pais Pais { get; set; }
        public List<Canton> Cantones { get; private set; }
        

        public Provincia()
        {
            Cantones = new List<Canton>(); 
        }
        
        protected bool Equals(Provincia other)
        {
            return Nombre == other.Nombre && NombrePais == other.NombrePais;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Provincia) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, NombrePais);
        }
    }
}
