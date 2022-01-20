using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.CheckLists
{
    public class MultimediaContentItemRepository : IMultimediaContentItemRepository
    {
        protected readonly ISqlDataProvider _db;

        public MultimediaContentItemRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task<IEnumerable<MultimediaContent>> GetByCheckListItem(int itemId, int listId, string incidentCode)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.ExecuteQueryAsync<MultimediaContent>(
                    @"
                        select MC.*
                        from MultimediaContentItem MCI
                        join MultimediaContent MC on MC.Id = MCI.Id_MultCont
                        where Id_Item = @Id_Item and Id_Lista = @Id_Lista and Codigo_Incidente = @Codigo_Incidente
                    ",
                    new 
                    { 
                        Id_Item = itemId,
                        Id_Lista = listId,
                        Codigo_Incidente = incidentCode
                    }
                );
            }
        }

        public async Task<MultimediaContentItem> InsertAsync(MultimediaContentItem mcItem)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.InsertAsync(mcItem);
                return mcItem;
            }
        }
    }
}
