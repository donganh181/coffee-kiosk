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

        public ICategoryRepository CategoryRepository { get; set; }

        public IProductRepository ProductRepository { get; set; }

        public IAreaRepository AreaRepository { get; set; }

        public ICampaignRepository CampaignRepository { get; set; }

        public IProductImageRepository ProductImageRepository { get; set; }

        public UnitOfWork(Coffee_KioskContext context, IAccountRepository accountRepository,
            ICategoryRepository categoryRepository, IProductRepository productRepository,
            IProductImageRepository productImageRepository)
        {
            _context = context;
            AccountRepository = accountRepository;
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
            ProductImageRepository = productImageRepository;
        }

        /*public UnitOfWork(Coffee_KioskContext context, IAreaRepository areaRepository,
            ICampaignRepository campaignRepository)
        {
            _context = context;
            AreaRepository = areaRepository;
            CampaignRepository = campaignRepository;
        }*/

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
