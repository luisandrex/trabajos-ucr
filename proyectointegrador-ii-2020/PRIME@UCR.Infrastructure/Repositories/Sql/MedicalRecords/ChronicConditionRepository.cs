using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords
{
    public class ChronicConditionRepository : GenericRepository<PadecimientosCronicos, int>, IChronicConditionRepository
    {
        public ChronicConditionRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }
        public override async Task<IEnumerable<PadecimientosCronicos>> GetByConditionAsync(Expression<Func<PadecimientosCronicos, bool>> expression)
        {
            return await _db.ChronicCondition
                .Include(e => e.Expediente)
                .Include(e => e.ListaPadecimiento).Where(expression).ToListAsync();
        }

        public async Task DeleteByIdsAsync(int recordId, int listId)
        {
            var borrado = _db.Set<PadecimientosCronicos>().Find(recordId, listId);
            if (borrado != null)
            {
                _db.Set<PadecimientosCronicos>().Remove(borrado);
            }
            await _db.SaveChangesAsync();
        }
    }
}
