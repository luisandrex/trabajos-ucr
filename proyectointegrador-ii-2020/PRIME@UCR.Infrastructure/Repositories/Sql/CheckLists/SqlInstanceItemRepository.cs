using PRIME_UCR.Application.Repositories;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Domain.Models;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using PRIME_UCR.Domain.Models.CheckLists;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RepoDb;
using System.Linq;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.CheckLists
{
    class SqlInstanceItemRepository : RepoDbRepository<InstanciaItem, int>, IInstanceItemRepository
    {
        public SqlInstanceItemRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public async Task<IEnumerable<InstanciaItem>> GetByIncidentCodAndCheckListId(string incidentCode, int checklistId)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.ExecuteQueryAsync<InstanciaItem>(
                    @"
                    select *
                    from InstanciaItem
                    where Codigo_Incidente = @IncidentCode and Id_Lista = @ChecklistId",
                    new { IncidentCode = incidentCode, ChecklistId = checklistId }
                );
            }
        }

        public async Task<IEnumerable<InstanciaItem>> GetCoreItems(string incidentCode, int checklistId)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.ExecuteQueryAsync<InstanciaItem>(
                    @"
                    select *
                    from InstanciaItem
                    where Codigo_Incidente = @IncidentCode and Id_Lista = @ChecklistId and Id_Item_Padre is null",
                    new { IncidentCode = incidentCode, ChecklistId = checklistId }
                );
            }
        }

        public async Task<IEnumerable<InstanciaItem>> GetItemsByFatherId(string incidentCode, int checklistId, int itemId)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.ExecuteQueryAsync<InstanciaItem>(
                    @"
                    select *
                    from InstanciaItem
                    where Codigo_Incidente_Padre = @IncidentCode and Id_Lista_Padre = @ChecklistId and Id_Item_Padre = @ItemId",
                    new { IncidentCode = incidentCode, ChecklistId = checklistId, ItemId = itemId }
                );
            }
        }
        public override async Task UpdateAsync(InstanciaItem item)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.UpdateAsync(item, i => i.ItemId == item.ItemId && i.PlantillaId == item.PlantillaId && i.IncidentCod == item.IncidentCod);
            }
        }


        public async Task<InstanciaItem> GetItem(int? itemId, string incidentCod, int? plantillaId)
        {
            IEnumerable<InstanciaItem> items;
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                items = await connection.ExecuteQueryAsync<InstanciaItem>(
                    @"
                    select *
                    from InstanciaItem
                    where Codigo_Incidente = @IncidentCode and Id_Lista = @ChecklistId and Id_Item = @ItemId",
                    new { IncidentCode = incidentCod, ChecklistId = plantillaId, ItemId = itemId }
                );
            }
            return items.FirstOrDefault();
        }
    }
}