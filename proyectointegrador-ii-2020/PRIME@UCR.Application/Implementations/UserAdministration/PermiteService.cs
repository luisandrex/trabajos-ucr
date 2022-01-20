using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    internal class PermiteService : IPermiteService
    {
        private readonly IPermiteRepository _IPermiteRepository;

        public PermiteService(IPermiteRepository IPermiteRepository) 
        {
            _IPermiteRepository = IPermiteRepository;
        }

        public async Task DeletePermissionAsync(string idProfile, int idPermission)
        {
            await _IPermiteRepository.DeletePermissionAsync(idProfile, idPermission);
        }

        public async Task InsertPermissionAsync(string idProfile, int idPermission)
        {
            await _IPermiteRepository.InsertPermissionAsync(idProfile, idPermission);
        }
    }
}
