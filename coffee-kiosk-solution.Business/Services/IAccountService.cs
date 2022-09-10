﻿using coffee_kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface IAccountService
    {
        Task<AccountViewModel> Create (Guid creatorId, AccountCreateViewModel model);
        Task<AccountViewModel> Login(LoginViewModel model);
    }
}
