using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Dtos.Incidents
{
    public class InternationalModel
    {
        public Pais Country { get; set; }

        public InternationalModel Clone()
        {
            return new InternationalModel
            {
                Country = Country
            };
        }
    }
}