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
    class SqlCheckListTypeRepository : GenericRepository<TipoListaChequeo, int>, ICheckListTypeRepository
    {
        public SqlCheckListTypeRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }
    }
}
