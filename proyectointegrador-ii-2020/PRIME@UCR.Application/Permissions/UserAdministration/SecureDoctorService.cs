using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Permissions.UserAdministration
{
    public class SecureDoctorService : IDoctorService
    {
        private readonly DoctorService doctorService;

        private readonly IPrimeSecurityService primeSecurityService;

        private readonly IDoctorRepository doctorRepository;

        public SecureDoctorService(IPrimeSecurityService _primeSecurityService,
            IDoctorRepository _doctorRepository)
        {
            doctorRepository = _doctorRepository;
            primeSecurityService = _primeSecurityService;
            doctorService = new DoctorService(doctorRepository);
        }

        public async Task<Médico> GetDoctorByIdAsync(string id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeBasicDetailsOfIncidents });
            return await doctorService.GetDoctorByIdAsync(id);
        }

        public async Task<IEnumerable<Médico>> GetAllDoctorsAsync()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeBasicDetailsOfIncidents });
            return await doctorService.GetAllDoctorsAsync();
        }
    }
}
