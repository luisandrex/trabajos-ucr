using System;
using PRIME_UCR.Domain.Models;
using System.Collections.Generic;

namespace PRIME_UCR.Domain.Models
{
    public class Distrito
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdCanton { get; set; }
        public Canton Canton { get; set; }
        public List<CentroMedico> CentroMedicos { get; private set; }
        public List<Domicilio> Domicilios { get; private set; }
        
        public Distrito()
        {
            CentroMedicos = new List<CentroMedico>();
            Domicilios = new List<Domicilio>();
        }
        
        protected bool Equals(Distrito other)
        {
            return Id == other.Id && Nombre == other.Nombre && IdCanton == other.IdCanton;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Distrito) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, IdCanton);
        }

    }
}