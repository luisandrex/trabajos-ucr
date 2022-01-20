namespace PRIME_UCR.Domain.Models
{
    public class Internacional : Ubicacion
    {
        public string NombrePais { get; set; }
        public Pais Pais { get; set; }
        public override string DisplayName => "Internacional";
    }
}