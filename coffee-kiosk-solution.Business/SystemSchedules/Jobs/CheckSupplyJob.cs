using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.SystemSchedules.Jobs
{
    public class CheckSupplyJob : IJob
    {
        private readonly ILogger<CheckSupplyJob> _logger;

        public CheckSupplyJob(ILogger<CheckSupplyJob> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //check time service
            /*if (!check)
            {
                _logger.LogError("Server Error.");
            }*/
            _logger.LogInformation("check template job running...");
        }
    }
}
