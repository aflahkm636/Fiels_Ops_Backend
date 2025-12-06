using Field_ops.Domain.Enums;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO;

public class ServiceTasksService : IServiceTasksService
{
    private readonly IServiceTasksRepository _repo;

    public ServiceTasksService(IServiceTasksRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> CreateAsync(ServiceTaskCreateDto dto)
    {
        if (dto.SubscriptionId is null && dto.ComplaintId is null)
            throw new ArgumentException("SubscriptionId or ComplaintId is required.");

        if (dto.SubscriptionId is not null && dto.ComplaintId is not null)
            throw new ArgumentException("Only one of SubscriptionId or ComplaintId can be provided.");

        return await _repo.CreateAsync(dto);
    }

    public async Task<IEnumerable<dynamic>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<dynamic?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid Id");

        return await _repo.GetByIdAsync(id);
    }

    public async Task<int> UpdateAsync(ServiceTaskUpdateDto dto)
    {
        if (dto.Id <= 0)
            throw new ArgumentException("Task Id required.");

        return await _repo.UpdateAsync(dto);
    }

    public async Task<int> UpdateStatusAsync(ServiceTaskUpdateStatusDto dto)
    {
        if (dto.Id <= 0)
            throw new ArgumentException("Task Id required.");

        return await _repo.UpdateStatusAsync(dto);
    }

    public async Task<int> DeleteAsync(int id, int actionUserId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid Id");

        return await _repo.DeleteAsync(id, actionUserId);
    }

    public async Task<IEnumerable<dynamic>> GetTasksByCustomerAsync(int actionUserId)
    {
        if (actionUserId <= 0)
            throw new ArgumentException("Invalid ActionUserId");

        return await _repo.GetTasksByCustomerAsync(actionUserId);
    }

    public async Task<IEnumerable<dynamic>> GetTasksByStatusAsync(ServiceTaskStatus status)
    {
        return await _repo.GetTasksByStatusAsync(status.ToString());
    }

    public async Task<IEnumerable<dynamic>> GetTasksByTechnicianAsync(int employeeId)
    {
        if (employeeId <= 0)
            throw new ArgumentException("Invalid EmployeeId");

        return await _repo.GetTasksByTechnicianAsync(employeeId);
    }

    public async Task<IEnumerable<dynamic>> GetTasksBySubscriptionIdAsync(int subscriptionId)
    {
        if (subscriptionId <= 0)
            throw new ArgumentException("Invalid SubscriptionId");

        return await _repo.GetTasksBySubscriptionIdAsync(subscriptionId);
    }
}
