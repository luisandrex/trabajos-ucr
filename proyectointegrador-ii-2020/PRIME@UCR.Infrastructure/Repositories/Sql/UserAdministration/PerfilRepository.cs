using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using RepoDb; 
using RepoDb.Extensions;
using System.Data.SqlClient; 
using System.Linq;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    public class PerfilRepository :  IPerfilRepository
    {
        protected readonly ISqlDataProvider _db;
        public PerfilRepository(ISqlDataProvider dataProvider) 
        {
            _db = dataProvider;
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

        /*
         * Function: Determines if an user exists in AdministradorCentroDeControl table in the Database
         * @Params: The user´s id (Cedula)
         * @Return: True if it exists in the table; False otherwise
         * @Story ID: PIG01IIC20-712
         */
        public async Task<bool> IsAdministratorAsync(string id) 
        {
            using (var connection = new SqlConnection(_db.ConnectionString)) 
            { 
                IEnumerable<AdministradorCentroDeControl> admin = await connection.QueryAsync<AdministradorCentroDeControl>(id);
                return admin.Count() != 0;
            } 
        }

        /*
         * Function: Determines if an user exists in CoordinadorTécnicoMédico table in the Database
         * @Params: The user´s id (Cedula)
         * @Return: True if it exists in the table; False otherwise
         * @Story ID: PIG01IIC20-712
         */
        public async Task<bool> IsCoordinatorAsync(string id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString)) 
            { 
                IEnumerable<CoordinadorTécnicoMédico> coordinator = await connection.QueryAsync<CoordinadorTécnicoMédico>(id);
                return coordinator.Count() != 0;
            } 
        }

        /*
         * Function: Determines if an user exists in Médico table in the Database
         * @Params: The user´s id (Cedula)
         * @Return: True if it exists in the table; False otherwise
         * @Story ID: PIG01IIC20-712
         */
        public async Task<bool> IsDoctorAsync(string id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString)) 
            { 
                IEnumerable<Médico> doctor = await connection.QueryAsync<Médico>(id);
                return doctor.Count() != 0;
            } 
        }

        /*
         * Function: Determines if an user exists in EspecialistaTécnicoMédico table in the database
         * @Params: The user´s id (Cedula)
         * @Return: True if it exists in the table; False otherwise
         * @Story ID: PIG01IIC20-712
         */
        public async Task<bool> IsTechnicalSpecialistAsync(string id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString)) 
            { 
                IEnumerable<EspecialistaTécnicoMédico> specialist = await connection.QueryAsync<EspecialistaTécnicoMédico>(id);
                return specialist.Count() != 0;
            } 
        }
    }
}

