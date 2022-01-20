namespace PRIME_UCR.Domain.Exceptions.Incidents
{
    public class DuplicateMedicalCenterException : DomainException
    {
        public DuplicateMedicalCenterException()
            : base("No puede asignar el mismo centro médico en origen y en destino.")
        {
        }

    }
}