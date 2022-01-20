using PRIME_UCR.Domain.Models.UserAdministration;
using System;

namespace PRIME_UCR.Domain.Models
{
    public class DocumentacionIncidente
    {
        public int Id { get; set; }
        public Incidente Incidente { get; set; }
        public string CodigoIncidente { get; set; }
        public string RazonDeRechazo { get; set; }
    }
}