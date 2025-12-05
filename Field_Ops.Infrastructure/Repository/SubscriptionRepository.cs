using Dapper;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.DTO.SubscriptionDto;
using System.Data;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly IDbConnection _db;

    public SubscriptionRepository(IDbConnection db)
    {
        _db = db;
    }

    public Task<int> CreateAsync(SubscriptionCreateDto dto)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "CREATE");
        p.Add("@CustomerId", dto.CustomerId);
        p.Add("@PlanId", dto.PlanId);
        p.Add("@MachineProductId", dto.MachineProductId);
        p.Add("@StartDate", dto.StartDate);
        p.Add("@EndDate", dto.EndDate);
        p.Add("@Status", dto.Status);
        p.Add("@LastServiceDate", dto.LastServiceDate);
        p.Add("@CreatedBy", dto.CreatedBy);

        return _db.QueryFirstAsync<int>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<IEnumerable<dynamic>> GetAllAsync()
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "GETALL");

        return _db.QueryAsync<dynamic>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<dynamic?> GetByIdAsync(int id)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "GETBYID");
        p.Add("@Id", id);

        return _db.QueryFirstOrDefaultAsync<dynamic>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    // GET BY CUSTOMER (GETBYUSER in SP)
    public Task<IEnumerable<dynamic>> GetByCustomerIdAsync(int customerId)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "GETBYUSER");
        p.Add("@CustomerId", customerId);

        return _db.QueryAsync<dynamic>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<int> UpdateAsync(SubscriptionUpdateDto dto)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "UPDATE");
        p.Add("@Id", dto.Id);
        p.Add("@PlanId", dto.PlanId);
        p.Add("@MachineProductId", dto.MachineProductId);
        p.Add("@StartDate", dto.StartDate);
        p.Add("@EndDate", dto.EndDate);
        p.Add("@Status", dto.Status);
        p.Add("@LastServiceDate", dto.LastServiceDate);
        p.Add("@ModifiedBy", dto.ModifiedBy);

        return _db.QueryFirstAsync<int>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<int> DeleteAsync(int id, int deletedBy)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "DELETE");
        p.Add("@Id", id);
        p.Add("@DeletedBy", deletedBy);

        return _db.QueryFirstAsync<int>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<int> PauseAsync(int id, int modifiedBy)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "PAUSE");
        p.Add("@Id", id);
        p.Add("@ModifiedBy", modifiedBy);

        return _db.QueryFirstAsync<int>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<int> ResumeAsync(int id, int modifiedBy)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "RESUME");
        p.Add("@Id", id);
        p.Add("@ModifiedBy", modifiedBy);

        return _db.QueryFirstAsync<int>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<int> CancelAsync(int id, int modifiedBy, DateTime? endDate)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "CANCEL");
        p.Add("@Id", id);
        p.Add("@ModifiedBy", modifiedBy);
        p.Add("@EndDate", endDate);

        return _db.QueryFirstAsync<int>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }

    public Task<int> AutoExpireAsync(int id, int modifiedBy)
    {
        var p = new DynamicParameters();
        p.Add("@FLAG", "AUTOEXPIRE");
        p.Add("@Id", id);
        p.Add("@ModifiedBy", modifiedBy);

        return _db.QueryFirstAsync<int>(
            "SP_SUBSCRIPTIONS",
            p,
            commandType: CommandType.StoredProcedure
        );
    }
}
