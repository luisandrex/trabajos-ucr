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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    internal class UsuarioRepository : IUsuarioRepository
    {
        private readonly ISqlDataProvider _db;

        public UsuarioRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        /**
         * Method used to get the list of all users with details.
         * 
         * Return: List of users with details.
         */
        public async Task<List<Usuario>> GetAllUsersWithDetailsAsync()
        {
            return await _db.Usuarios
            .Include(u => u.Persona)
            .Include(u => u.UsuariosYPerfiles)
            .ThenInclude(p => p.Perfil)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await _db.Usuarios
            .Include(u => u.Persona)
            .Include(u => u.UsuariosYPerfiles)
            .FirstAsync(u => u.Email == email);
        }

        public async Task<Usuario> GetWithDetailsAsync(string id)
        {
            return await _db.Usuarios
            .Include(u => u.UsuariosYPerfiles)
            .Include(u => u.Persona)
            .FirstAsync(u => u.Id == id);
        }

        public async Task<List<Usuario>> GetNotAuthenticatedUsers()
        {
            return await _db.Usuarios
            .Include(u => u.Persona)
            .Include(u => u.UsuariosYPerfiles)
            .ThenInclude(p => p.Perfil)
            .Where( u => !u.EmailConfirmed)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
