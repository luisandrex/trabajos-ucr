using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    internal class PersonService : IPersonService
    {

        private readonly IPersonaRepository PersonRepository;

        public PersonService(IPersonaRepository _personaRepository)
        {
            PersonRepository = _personaRepository;
        }

        /**
         * Method used to get a person by its id, in this case, by its Cedula
         */
        public async Task<Persona> GetPersonByIdAsync(string id)
        {
            return await PersonRepository.GetByKeyPersonaAsync(id);
        }

        /**
         * Method used to convert a RegisterUserForm to a Person DTO
         * 
         * Return:  A person DTO with its information.
         */
        public async Task<PersonFormModel> GetPersonModelFromRegisterModelAsync(RegisterUserFormModel registerUserModel)
        {
            if (registerUserModel != null) { 
                PersonFormModel personModel = new PersonFormModel();
                personModel.IdCardNumber = registerUserModel.IdCardNumber;
                personModel.Name = registerUserModel.Name;
                personModel.FirstLastName = registerUserModel.FirstLastName;
                personModel.SecondLastName = registerUserModel.SecondLastName;
                personModel.Sex = registerUserModel.Sex.ToString();
                personModel.BirthDate = registerUserModel.BirthDate;
                personModel.PrimaryPhoneNumber = registerUserModel.PrimaryPhoneNumber;
                personModel.SecondaryPhoneNumber = registerUserModel.SecondaryPhoneNumber;
                return await Task.FromResult(personModel);
            }
            return null; 
        }

        /**
         * Method used to convert a person DTO to the persona model
         * 
         * Return: A person model with the information given by the user
         */
        private Persona GetPersonaFromPersonModel(PersonFormModel personInfo)
        {
            Persona person = new Persona();
            person.Cédula = personInfo.IdCardNumber;
            person.FechaNacimiento = personInfo.BirthDate;
            person.Nombre = personInfo.Name;
            person.PrimerApellido = personInfo.FirstLastName;
            person.SegundoApellido = personInfo.SecondLastName;
            person.Sexo = personInfo.Sex;
            return person;
        }

        /**
         * Method used to store a person in the database
         */
        public async Task StoreNewPersonAsync(PersonFormModel personInfo)
        {
            var person = GetPersonaFromPersonModel(personInfo);
            await PersonRepository.InsertAsync(person);
        }

        /**
         * Method used to delete a person from the database
         */
        public async Task DeletePersonAsync(string id)
        {
            await PersonRepository.DeleteAsync(id);
        }

        /**
         * Method used to get a person by its id, in this case, by its Cedula. It's used by the user with permissions to manage users.
         */
        public async Task<Persona> GetPersonByCedAsync(string ced)
        {
            return await PersonRepository.GetByCedPersonaAsync(ced);
        }
    }
}
