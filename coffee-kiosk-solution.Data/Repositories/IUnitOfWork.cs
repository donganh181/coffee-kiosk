using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.Repositories
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
