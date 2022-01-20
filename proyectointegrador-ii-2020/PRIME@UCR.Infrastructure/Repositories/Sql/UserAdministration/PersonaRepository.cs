using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Permissions.UserAdministration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    internal class PersonaRepository : IPersonaRepository
    {
        private readonly ISqlDataProvider _db;

        public PersonaRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task DeleteAsync(string cedPersona)
        {
            var person = await _db.People.FindAsync(cedPersona);
            if(person != null)
            {
                _db.People.Remove(person);
            }
            await _db.SaveChangesAsync();
        }

        public async Task<Persona> GetByCedPersonaAsync(string ced)
        {
            return await _db.People.FindAsync(ced);
        }

        public async Task<Persona> GetByKeyPersonaAsync(string id)
        {
            return await _db.People.FindAsync(id);
        }

        public async Task<Persona> GetWithDetailsAsync(string id)
        {
            return await _db.People
                    .Include(i => i.Cédula)
                    .Include(i => i.Nombre)
                    .Include(i => i.PrimerApellido)
                    .Include(i => i.SegundoApellido)
                    .Include(i => i.Sexo)
                    .FirstOrDefaultAsync(i => i.Cédula == id);
        }

        public async Task InsertAsync(Persona persona)
        {
            _db.People.Add(persona);
            await _db.SaveChangesAsync();
        }
    }
}
