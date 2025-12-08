using Dapper;
using Field_Ops.Application.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Infrastructure.Repository
{
    public class AutomationRepository : IAutomationRepository
    {
        private readonly IDbConnection _db;

        public AutomationRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<dynamic>> GetSubscriptionsToExpireAsync()
        {
            var sql = @"
            SELECT Id FROM Subscriptions
            WHERE IsDeleted = 0
              AND EndDate <= CAST(SYSUTCDATETIME() AS DATE)
              AND Status != 'Expired'";

            return await _db.QueryAsync<dynamic>(sql);
        }

        public async Task<IEnumerable<dynamic>> GetSubscriptionsToRenewAsync()
        {
            var sql = @"
            SELECT S.Id
            FROM Subscriptions S
            JOIN SubscriptionPlans P ON S.PlanId = P.Id
            WHERE S.IsDeleted = 0
              AND S.Status IN ('Expired', 'RenewPending')
              AND P.MonthlyCharge > 0";

            return await _db.QueryAsync<dynamic>(sql);
        }

        public async Task<IEnumerable<dynamic>> GetSubscriptionsDueForServiceAsync()
        {
            var sql = @"
            SELECT S.Id
            FROM Subscriptions S
            JOIN SubscriptionPlans P ON S.PlanId = P.Id
            WHERE S.IsDeleted = 0
              AND S.LastServiceDate IS NOT NULL
              AND DATEADD(DAY, P.FrequencyInDays, S.LastServiceDate)
                  <= CAST(SYSUTCDATETIME() AS DATE)
              AND S.Status != 'ServiceDue'";

            return await _db.QueryAsync<dynamic>(sql);
        }

        public async Task<bool> AutoExpireAsync(int id, int modifiedBy)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "AUTOEXPIRE");
            p.Add("@Id", id);
            p.Add("@ModifiedBy", modifiedBy);

            await _db.ExecuteAsync("SP_SUBSCRIPTIONS", p, commandType: CommandType.StoredProcedure);
            return true;
        }

        public async Task<bool> AutoRenewAsync(int id, int modifiedBy)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "AUTORENEW");
            p.Add("@Id", id);
            p.Add("@ModifiedBy", modifiedBy);

            await _db.ExecuteAsync("SP_SUBSCRIPTIONS", p, commandType: CommandType.StoredProcedure);
            return true;
        }

        public async Task<bool> MarkServiceDueAsync(int id, int modifiedBy)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "AUTOSERVICE_DUE");
            p.Add("@Id", id);
            p.Add("@ModifiedBy", modifiedBy);

            await _db.ExecuteAsync("SP_SUBSCRIPTIONS", p, commandType: CommandType.StoredProcedure);
            return true;
        }

        public async Task<bool> AutoCreateServiceTaskAsync(int subscriptionId, string? notes)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "AUTO_CREATE");
            p.Add("@SubscriptionId", subscriptionId);
            p.Add("@Notes", notes);

            await _db.ExecuteAsync("SP_SERVICETASKS", p, commandType: CommandType.StoredProcedure);
            return true;
        }
    }

}
