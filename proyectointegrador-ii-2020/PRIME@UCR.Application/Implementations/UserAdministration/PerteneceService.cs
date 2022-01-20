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
    internal class PerteneceService : IPerteneceService
    {
        private readonly IPerteneceRepository _perteneceRepository;

        public PerteneceService(IPerteneceRepository perteneceRepository)
        {
            _perteneceRepository = perteneceRepository;
        }

        public async Task DeleteUserOfProfileAsync(string idUser, string idProfile)
        {
            await _perteneceRepository.DeleteUserFromProfileAsync(idUser, idProfile);
        }

        public async Task InsertUserOfProfileAsync(string idUser, string idProfile)
        {
            await _perteneceRepository.InsertUserToProfileAsync(idUser, idProfile);
        }
    }
}
