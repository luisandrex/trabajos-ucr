namespace PRIME_UCR.Domain.Models
{
    public abstract class Ubicacion
    {
        public int Id { get; set; }
        public abstract string DisplayName { get; }
    }
}