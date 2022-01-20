using PRIME_UCR.Domain.Models.CheckLists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.CheckLists
{
    public interface ICheckListRepository : IGenericRepository<CheckList, int>
    {
        Task<IEnumerable<CheckList>> GetByName(string name);
        Task<CheckList> InsertCheckListAsync(CheckList list);
        Task<IEnumerable<CheckList>> GetActivated();
    }
}
