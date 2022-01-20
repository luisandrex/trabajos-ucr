namespace PRIME_UCR.Domain.Exceptions.Incidents
{
    public class DuplicateAssignedDoctorException : DomainException
    {
        public DuplicateAssignedDoctorException()
            : base("No puede asignar a la misma persona como médico en origen y en destino.")
        {
        }
    }
}