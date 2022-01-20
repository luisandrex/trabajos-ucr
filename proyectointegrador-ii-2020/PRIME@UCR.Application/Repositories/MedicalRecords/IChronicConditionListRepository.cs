using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Application.Repositories;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords
{
    public interface IChronicConditionListRepository : IGenericRepository<ListaPadecimiento, int>
    {

    }
}

