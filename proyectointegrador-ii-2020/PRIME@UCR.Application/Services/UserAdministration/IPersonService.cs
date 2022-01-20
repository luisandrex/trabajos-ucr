using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IPersonService
    {
        Task<Persona> GetPersonByCedAsync(string ced);

        Task<Persona> GetPersonByIdAsync(string id);

        Task StoreNewPersonAsync(PersonFormModel personInfo);

        Task<PersonFormModel> GetPersonModelFromRegisterModelAsync(RegisterUserFormModel registerUserModel);

        Task DeletePersonAsync(string id);
    }
}
