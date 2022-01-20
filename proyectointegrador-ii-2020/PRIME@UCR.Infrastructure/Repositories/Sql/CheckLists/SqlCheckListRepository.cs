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

namespace PRIME_UCR.Infrastructure.Repositories.Sql.CheckLists
{
    class SqlCheckListRepository : GenericRepository<CheckList, int>, ICheckListRepository
    {
        public SqlCheckListRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public async Task<CheckList> InsertCheckListAsync(CheckList list)
        {
            await using var connection = new SqlConnection(_db.ConnectionString);
            var parameters = new Dictionary<string, object>
            {
                {"nombre", list.Nombre},
                {"tipo", list.Tipo},
                {"descripcion", list.Descripcion},
                {"orden", list.Orden},
                {"imagenDescriptiva", list.ImagenDescriptiva},
                {"editable",list.Editable },
                {"activada",list.Activada }
            };
            var result = await connection.ExecuteScalarAsync(
                "dbo.InsertarListaChequeo", parameters, CommandType.StoredProcedure);

            list.Id = int.Parse(s: result.ToString());

            return list;
        }

        public async Task<IEnumerable<CheckList>> GetByName(string name)
        {
            return await this.GetByConditionAsync(checkListModel => checkListModel.Nombre == name);
        }
        public async Task<IEnumerable<CheckList>> GetActivated()
        {
            return await this.GetByConditionAsync(checkListModel => checkListModel.Activada == true);
        }
    }
}
