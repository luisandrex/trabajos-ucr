using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;
using RepoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ISqlDataProvider _db;

        public AuthenticationRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task<List<Usuario>> GetAllUsersWithDetailsAsync()
        {
            return await _db.Usuarios
                    .Include(u => u.Persona)
                    .Include(u => u.UsuariosYPerfiles)
                    .ToListAsync();
        }

        public async Task<List<Perfil>> GetPerfilesWithDetailsAsync()
        {
            var profiles = new List<Perfil>();
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var extractor = await connection.ExecuteQueryMultipleAsync(@"
                    SELECT *
                    FROM Usuario as U
                        JOIN AspNetUsers as ANU ON U.Id = ANU.Id;

                    SELECT P.*
                    FROM Usuario U
                    JOIN AspNetUsers ASP on U.Id = ASP.Id
                    JOIN Pertenece P on P.IdUsuario = U.Id;

                    SELECT PER.*
                    FROM Perfil PERF
                    JOIN Permite PER on PER.NombrePerfil = PERF.NombrePerfil;

                    SELECT *
                    FROM Permiso;

                    SELECT *
                    FROM Perfil;
                ");

                var users = extractor.Extract<Usuario>().AsList();
                var userProfiles = extractor.Extract<Pertenece>().AsList();
                var allowed = extractor.Extract<Permite>().AsList();
                var permissions = extractor.Extract<Permiso>().AsList();
                profiles = extractor.Extract<Perfil>().AsList();

                userProfiles.ForEach(userProfile =>
                    userProfile.Usuario = users.FirstOrDefault(u => u.Id == userProfile.IDUsuario));

                allowed.ForEach(allow =>
                    allow.Permiso = permissions.FirstOrDefault(p => p.IDPermiso == allow.IDPermiso));

                profiles.ForEach(profile => {
                    profile.UsuariosYPerfiles = userProfiles.Where(userProfile => userProfile.IDPerfil == profile.NombrePerfil).ToList();
                    profile.PerfilesYPermisos = allowed.Where(allow => allow.IDPerfil == profile.NombrePerfil).ToList();
                });

            }

            return profiles;
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var returnedValue = await connection.ExecuteQueryMultipleAsync(@"
                    SELECT *
                    FROM Usuario U
                        JOIN AspNetUsers NetU on U.Id = NetU.Id
                    WHERE NetU.Email = @Email;

                    SELECT P.*
                    FROM Usuario U
                        JOIN AspNetUsers ASP on U.Id = ASP.Id
                        JOIN Pertenece P on P.IdUsuario = U.Id
                    WHERE ASP.Email = @Email;

                    SELECT Pe.*
                    FROM Usuario U
                        JOIN AspNetUsers ASP on U.Id = ASP.Id
                        JOIN Pertenece P on P.IdUsuario = U.Id
                        JOIN Persona Pe on Pe.Cédula = U.CédulaPersona
                    WHERE ASP.Email = @Email;
                ", new { Email = email });

                var user = returnedValue.Extract<Usuario>().FirstOrDefault();
                if(user != null)
                {
                    user.UsuariosYPerfiles = returnedValue.Extract<Pertenece>().AsList();
                    user.Persona = returnedValue.Extract<Persona>().FirstOrDefault();
                }
                return user;
            }
        }

        public async Task<Usuario> GetWithDetailsAsync(string id)
        {
            return await _db.Usuarios
                .Include(u => u.UsuariosYPerfiles)
                .Include(u => u.Persona)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
