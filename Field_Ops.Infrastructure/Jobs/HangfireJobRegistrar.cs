using Hangfire;
using Field_Ops.Application.Contracts.Service;

namespace Field_Ops.WebApi.Jobs
{
    public static class HangfireJobRegistrar
    {
        public static void RegisterRecurringJobs()
        {
            // Run Expiry every day at midnight
            RecurringJob.AddOrUpdate<IAutomationService>(
                "auto-expire",
                service => service.RunAutoExpire(),
                 Cron.Daily(9));

            // Run auto-renew every day at 00:05
            RecurringJob.AddOrUpdate<IAutomationService>(
                "auto-renew",
                service => service.RunAutoRenew(),
                "0 9 * * *");

            // Run Auto Service Due every hour
            RecurringJob.AddOrUpdate<IAutomationService>(
                "auto-service-due",
                service => service.RunAutoServiceDue(),
                Cron.Hourly);
        }
    }
}
