using System.Collections.Generic;

namespace PRIME_UCR.Domain.Models
{
    public class Pais
    {
        public const string DefaultCountry = "Costa Rica";
        
        public Pais()
        {
            Provincias = new List<Provincia>();
            PaisUbicaciones = new List<Internacional>();
        }

        protected bool Equals(Pais other)
        {
            return Nombre == other.Nombre;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pais) obj);
        }

        public override int GetHashCode()
        {
            return (Nombre != null ? Nombre.GetHashCode() : 0);
        }

        public string Nombre { get; set; }
        public List<Provincia> Provincias { get; private set; }
        public List<Internacional> PaisUbicaciones { get; private set; }
    }
}