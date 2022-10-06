using coffee_kiosk_solution.Business.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.SystemSchedules.Jobs
{
    public class CheckSupplyJob : IJob
    {
        private readonly ILogger<CheckSupplyJob> _logger;
        private readonly ISupplyService _supplyService;

        public CheckSupplyJob(ILogger<CheckSupplyJob> logger, ISupplyService supplyService)
        {
            _logger = logger;
            _supplyService = supplyService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var check = await _supplyService.CheckSupplyByWeek();
            if (!check)
            {
                _logger.LogError("Server Error.");
            }
            _logger.LogInformation("check supply job running...");
        }
    }
}
