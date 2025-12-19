using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.Contracts.Repository
{
    public interface IBillingRepository
    {
        Task GenerateAsync(int subscriptionId, DateTime billMonth, int systemUserId);
        Task<IEnumerable<dynamic>> GetPendingAsync();
        Task<dynamic?> GetByIdAsync(int id);
        Task<IEnumerable<dynamic>> GetByCustomerAsync(int customerId);
        Task<dynamic> UpdateDiscountAsync(int billingId, decimal discountPercent, int adminUserId);
        Task<dynamic> FinalizeAsync(int billingId, int adminUserId);
    }

}
