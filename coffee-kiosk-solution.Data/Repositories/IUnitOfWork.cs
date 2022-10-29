using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.Repositories
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IProductRepository ProductRepository { get; }
        IAreaRepository AreaRepository { get; }
        ICampaignRepository CampaignRepository { get; }
        IDiscountRepository DiscountRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IShopRepository ShopRepository { get; }
        ISupplyRepository SupplyRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IRoleRepository RoleRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
