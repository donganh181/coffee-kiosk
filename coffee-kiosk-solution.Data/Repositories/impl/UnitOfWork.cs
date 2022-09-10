using coffee_kiosk_solution.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.Repositories.impl
{
    public class UnitOfWork : IUnitOfWork
    {
        public Coffee_KioskContext _context { get; set; }

        public IAccountRepository AccountRepository { get; set; }

        public UnitOfWork(Coffee_KioskContext context, IAccountRepository accountRepository)
        {
            _context = context;
            AccountRepository = accountRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
