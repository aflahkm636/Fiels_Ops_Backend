using Hangfire;
using Field_Ops.Application.Contracts.Service;

namespace Field_Ops.WebApi.Jobs
{
    public static class HangfireJobRegistrar
    {
        public static void RegisterRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IAutomationService>(
                "auto-expire",
                service => service.RunAutoExpire(),
                 Cron.Daily(9));

            RecurringJob.AddOrUpdate<IAutomationService>(
                "auto-renew",
                service => service.RunAutoRenew(),
                "0 9 * * *");

            //RecurringJob.AddOrUpdate<IAutomationService>(
            //    "auto-service-due",
            //    service => service.RunAutoServiceDue(),
            //    Cron.Hourly);
        }
    }
}
