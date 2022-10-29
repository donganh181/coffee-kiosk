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

        public IDiscountRepository DiscountRepository { get; set; }

        public IProductImageRepository ProductImageRepository { get; set; }

        public IShopRepository ShopRepository { get; set; }

        public ISupplyRepository SupplyRepository { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderDetailRepository OrderDetailRepository { get; set; }

        public IRoleRepository RoleRepository { get; set; }

        public UnitOfWork(Coffee_KioskContext context, IAccountRepository accountRepository,
            ICategoryRepository categoryRepository, IProductRepository productRepository,
            IAreaRepository areaRepository, ICampaignRepository campaignRepository,
            IDiscountRepository discountRepository, IProductImageRepository productImageRepository,
            IShopRepository shopRepository, ISupplyRepository supplyRepository,
            IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository,
            IRoleRepository roleRepository)
        {
            _context = context;
            AccountRepository = accountRepository;
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
            AreaRepository = areaRepository;
            CampaignRepository = campaignRepository;
            DiscountRepository = discountRepository;
            ProductImageRepository = productImageRepository;
            ShopRepository = shopRepository;
            OrderRepository = orderRepository;
            OrderDetailRepository = orderDetailRepository;
            SupplyRepository = supplyRepository;
            RoleRepository = roleRepository;
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
