using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Permissions.UserAdministration;
using RepoDb;
using RepoDb.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    internal class PermisoRepository : IPermisoRepository
    {
        private readonly ISqlDataProvider _db; 

        public PermisoRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task<List<Permiso>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryMultipleAsync(@"
                    select * from Permiso;
                ");

                var profiles = result.Extract<Permiso>().AsList();

                return profiles;
            }
        }
    }
}
