using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    internal class ProfilesService : IProfilesService
    {
        private readonly IPerfilRepository _profilesRepository;

        public ProfilesService(IPerfilRepository profileRepository)
        {
            _profilesRepository = profileRepository;
        }

        public async Task<List<Perfil>> GetPerfilesWithDetailsAsync()
        {
            return await _profilesRepository.GetPerfilesWithDetailsAsync();
        }

        public async Task<bool> IsAdministratorAsync(string id)
        {
            return await _profilesRepository.IsAdministratorAsync(id);
        }

        public async Task<bool> IsCoordinatorAsync(string id)
        {
            return await _profilesRepository.IsCoordinatorAsync(id);
        }

        public async Task<bool> IsDoctorAsync(string id)
        {
            return await _profilesRepository.IsDoctorAsync(id);
        }

        public async Task<bool> IsTechnicalSpecialistAsync(string id)
        {
            return await _profilesRepository.IsTechnicalSpecialistAsync(id);
        }
    }
}
