using AutoMapper;
using AutoMapper.QueryableExtensions;
using coffee_kiosk_solution.Data.Repositories;
using coffee_kiosk_solution.Data.Responses;
using coffee_kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services.impl
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IRoleService> _logger;
        
        public RoleService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IRoleService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<RoleViewModel>> GetListRole()
        {
            var listRole = await _unitOfWork.RoleRepository.Get()
                .ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if(listRole == null)
            {
                _logger.LogError("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return listRole;
        }
    }
}
