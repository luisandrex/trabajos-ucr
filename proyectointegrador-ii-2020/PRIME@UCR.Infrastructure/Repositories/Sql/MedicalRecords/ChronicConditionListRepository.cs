using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords
{
    public class ChronicConditionListRepository : GenericRepository<ListaPadecimiento, int>, IChronicConditionListRepository
    {
        public ChronicConditionListRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }
    }

}
