using coffee_kiosk_solution.Data.Constants;
using coffee_kiosk_solution.Data.Context;
using coffee_kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Data.Repositories.impl
{
    public class OrderRepository : BaseRepository<TblOrder>, IOrderRepository
    {
        public OrderRepository(Coffee_KioskContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> UpdateStatus(Guid id)
        {
            var order = await dbContext.TblOrders.Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();

            order.Status = (int)OrderStatusConstants.Completed;
            try
            {
                dbContext.Attach(order);
                dbContext.Entry(order).Property(x => x.Status).IsModified = true;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
