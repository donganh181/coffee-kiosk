using AutoMapper;
using AutoMapper.QueryableExtensions;
using coffee_kiosk_solution.Business.Utilities;
using coffee_kiosk_solution.Data.Constants;
using coffee_kiosk_solution.Data.Models;
using coffee_kiosk_solution.Data.Repositories;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace coffee_kiosk_solution.Business.Services.impl
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IAccountService> _logger;

        public AccountService(IMapper mapper, IConfiguration configuration,
            IUnitOfWork unitOfWork, ILogger<IAccountService> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<AccountViewModel> Create(Guid creatorId, AccountCreateViewModel model)
        {
            var account = _mapper.Map<TblAccount>(model);

            account.CreatorId = creatorId;
            account.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            //add status
            try
            {
                await _unitOfWork.AccountRepository.InsertAsync(account);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.AccountRepository
                .Get(u => u.Username.Equals(model.Username))
                .Include(a => a.Role)
                .Include(b => b.Creator)
                .ProjectTo<AccountViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    _logger.LogError("Username is duplicated.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Username is duplicated.");
                }
                else
                {
                    _logger.LogError("Invalid data.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
                }
            }

        }

        public async Task<AccountViewModel> GetById(Guid id, string role, Guid checkId)
        {
            var user = await _unitOfWork.AccountRepository
                .Get(u => u.Id.Equals(id))
                .ProjectTo<AccountViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                _logger.LogError("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }

            if (!role.Equals(RoleConstants.ADMIN))
            {
                if (!user.Id.Equals(checkId))
                {
                    _logger.LogError("You cannot interact with other account.");
                    throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You cannot interact with other account.");
                }
            }
            return user;
        }

        public async Task<DynamicModelResponse<AccountSearchViewModel>> GetListAccount(AccountSearchViewModel model, int size, int pageNum)
        {
            var listAccount = _unitOfWork.AccountRepository
                .Get()
                .Include(a => a.Role)
                .Include(b => b.Creator)
                .ProjectTo<AccountSearchViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            var listPaging = listAccount
                .AsQueryable()
                .OrderByDescending(p => p.Username)
                .DynamicFilter(model)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogError("Cannot Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot Found");
            }

            var result = new DynamicModelResponse<AccountSearchViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total
                },
                Data = listPaging.Data.ToList()
            };
            return result;

        }

        public async Task<AccountViewModel> Login(LoginViewModel model)
        {
            var user = await _unitOfWork.AccountRepository
                .Get(u => u.Username.Equals(model.Username))
                .Include(a => a.Role)
                .Include(b => b.Creator)
                .ProjectTo<AccountViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (user == null || !BCryptNet.Verify(model.Password, user.Password))
            {
                _logger.LogError("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }

            //check banned or not

            try
            {
                user.Token = TokenUtil.GenerateJWTWebToken(user, _configuration);
                return user;

            }
            catch (Exception)
            {
                _logger.LogError("Server error.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server error.");
            }
        }
    }
}
