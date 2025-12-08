using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.services
{
    public class AutomationService : IAutomationService
    {
        private readonly IAutomationRepository _repo;

        public AutomationService(IAutomationRepository repo)
        {
            _repo = repo;
        }

        public async Task RunAutoExpire()
        {
            var list = await _repo.GetSubscriptionsToExpireAsync();
            foreach (var item in list)
            {
                await _repo.AutoExpireAsync((int)item.Id, modifiedBy: 1);
            }
        }

        public async Task RunAutoRenew()
        {
            var list = await _repo.GetSubscriptionsToRenewAsync();
            foreach (var item in list)
            {
                await _repo.AutoRenewAsync((int)item.Id, modifiedBy: 1);
            }
        }

        public async Task RunAutoServiceDue()
        {
            var list = await _repo.GetSubscriptionsDueForServiceAsync();

            foreach (var item in list)
            {
                int subId = (int)item.Id;

                await _repo.MarkServiceDueAsync(subId,1);

                await _repo.AutoCreateServiceTaskAsync(
                    subscriptionId: subId,
                    notes: "Auto-generated service task (Service Due)"
                );
            }
        }
    }

}
