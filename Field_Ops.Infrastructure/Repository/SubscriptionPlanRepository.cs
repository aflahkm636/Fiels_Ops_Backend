using Dapper;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.DTO.SubscriptionPlanDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Infrastructure.Repository
{
    public class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private readonly IDbConnection _db;

        public SubscriptionPlanRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(SubscriptionPlanCreateDto dto)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "CREATE");
            p.Add("@Name", dto.Name);
            p.Add("@Description", dto.Description);
            p.Add("@FrequencyInDays", dto.FrequencyInDays);
            p.Add("@MonthlyCharge", dto.MonthlyCharge);
            p.Add("@CreatedBy", dto.CreatedBy);

            var result = await _db.QueryFirstOrDefaultAsync<int>(
                "SP_SUBSCRIPTIONPLANS",
                p,
                commandType: CommandType.StoredProcedure
            );

            return result; 
        }

        public async Task<IEnumerable<dynamic>> GetAllAsync()
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "GETALL");

            return await _db.QueryAsync<dynamic>(
                "SP_SUBSCRIPTIONPLANS",
                p,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<dynamic> GetByIdAsync(int id)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "GETBYID");
            p.Add("@Id", id);

            var result = await _db.QueryFirstOrDefaultAsync<dynamic>(
                "SP_SUBSCRIPTIONPLANS",
                p,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }


        public async Task<bool> UpdateAsync(SubscriptionPlanUpdateDto dto)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "UPDATE");
            p.Add("@Id", dto.Id);
            p.Add("@Name", dto.Name);
            p.Add("@Description", dto.Description);
            p.Add("@FrequencyInDays", dto.FrequencyInDays);
            p.Add("@MonthlyCharge", dto.MonthlyCharge);
            p.Add("@ModifiedBy", dto.ModifiedBy);

            var rows = await _db.ExecuteAsync(
                "SP_SUBSCRIPTIONPLANS",
                p,
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id, int deletedBy)
        {
            var p = new DynamicParameters();
            p.Add("@FLAG", "DELETE");
            p.Add("@Id", id);
            p.Add("@DeletedBy", deletedBy);

            var rows = await _db.ExecuteAsync(
                "SP_SUBSCRIPTIONPLANS",
                p,
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }
    }

}
