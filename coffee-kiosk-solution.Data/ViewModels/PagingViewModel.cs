using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace coffee_kiosk_solution.Data.ViewModels
{
    public class PagingViewModel<TResult>
    {
        public int Total { get; set; }
        public IQueryable<TResult> Data { get; set; }
    }
}
