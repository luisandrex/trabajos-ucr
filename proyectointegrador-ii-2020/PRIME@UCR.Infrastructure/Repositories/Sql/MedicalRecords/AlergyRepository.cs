using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords
{
    public class AlergyRepository : GenericRepository<Alergias, int>, IAlergyRepository
    {
        public AlergyRepository(ISqlDataProvider dataProvider) : base(dataProvider) 
        {
        }
        public override async Task<IEnumerable<Alergias>> GetByConditionAsync(Expression<Func<Alergias, bool>> expression)
        {
            return await _db.Alergies
                .Include(e => e.Expediente)
                .Include(e => e.ListaAlergia).Where(expression).ToListAsync();
        }

        public async Task DeleteByIdsAsync(int recordId, int listId)
        {
            var borrado = _db.Set<Alergias>().Find(recordId, listId);
            if (borrado != null)
            {
                _db.Set<Alergias>().Remove(borrado);
            }
            await _db.SaveChangesAsync();
        }
    }
}
