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

        IProductImageRepository ProductImageRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
