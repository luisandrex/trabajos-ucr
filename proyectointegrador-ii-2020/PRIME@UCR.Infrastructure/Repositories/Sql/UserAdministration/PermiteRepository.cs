using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using System.Threading.Tasks;
using System.Data.Common;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using System.Reflection;
using PRIME_UCR.Infrastructure.Permissions.UserAdministration;
using System.ComponentModel.DataAnnotations;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    internal class PermiteRepository : IPermiteRepository
    {   
        private readonly ISqlDataProvider _db;

        public PermiteRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task DeletePermissionAsync(string idProfile, int idPermission) 
        {
            await Task.Run(() =>
            {
                using (var cmd = _db.DbConnection.CreateCommand())
                {
                    while (cmd.Connection.State != System.Data.ConnectionState.Open) 
                    {
                        cmd.Connection.Open();
                    }
                    if (cmd.Connection.State == System.Data.ConnectionState.Open) 
                    {
                        cmd.CommandText =
                            $"EXECUTE dbo.DeletePermissionFromProfile @idPermission='{idPermission}', @idProfile='{idProfile}'";

                        cmd.ExecuteNonQuery();
                    }
                    
                }
            });
        }
        public async Task InsertPermissionAsync(string idProfile, int idPermission)
        {
            await Task.Run(() =>
            {
                using (var cmd = _db.DbConnection.CreateCommand())
                {
                    while (cmd.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    if (cmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        cmd.CommandText =
                            $"EXECUTE dbo.InsertPermissionToProfile @idPermission='{idPermission}', @idProfile='{idProfile}'";

                        cmd.ExecuteNonQuery();
                    }

                }
            });
        }
    }
}
