using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.Contracts.Repository
{
    public interface IAutomationRepository
    {
        Task<IEnumerable<dynamic>> GetSubscriptionsToExpireAsync();
        Task<IEnumerable<dynamic>> GetSubscriptionsToRenewAsync();
        Task<IEnumerable<dynamic>> GetSubscriptionsDueForServiceAsync();

        Task<bool> AutoExpireAsync(int id, int modifiedBy);
        Task<bool> AutoRenewAsync(int id, int modifiedBy);
        Task<bool> MarkServiceDueAsync(int id, int modifiedBy);

        Task<bool> AutoCreateServiceTaskAsync(int subscriptionId, string? notes);
    }

}
