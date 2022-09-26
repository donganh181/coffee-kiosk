using coffee_kiosk_solution.Business.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.SystemSchedules.Jobs
{
    public class CheckCampaignJob : IJob
    {
        private readonly ILogger<CheckCampaignJob> _logger;
        private readonly ICampaignService _campaignService;

        public CheckCampaignJob(ILogger<CheckCampaignJob> logger, ICampaignService campaignService)
        {
            _logger = logger;
            _campaignService = campaignService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var check = await _campaignService.ValidateStatusOfCampaignByDay();
            if (!check)
            {
                _logger.LogError("Server Error.");
            }
            _logger.LogInformation("check supply job running...");
        }
    }
}
