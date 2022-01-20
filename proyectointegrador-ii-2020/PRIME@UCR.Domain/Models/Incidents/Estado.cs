using System.Collections.Generic;

namespace PRIME_UCR.Domain.Models.Incidents
{
    public class Estado
    {
        public Estado()
        {
            EstadoIncidentes = new List<EstadoIncidente>();
        }

        public string Nombre { get; set; }
        public List<EstadoIncidente> EstadoIncidentes { get; private set; }

        private sealed class NombreEqualityComparer : IEqualityComparer<Estado>
        {
            public bool Equals(Estado x, Estado y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Nombre == y.Nombre;
            }

            public int GetHashCode(Estado obj)
            {
                return (obj.Nombre != null ? obj.Nombre.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<Estado> NombreComparer { get; } = new NombreEqualityComparer();

    }
}