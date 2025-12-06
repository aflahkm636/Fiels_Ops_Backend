using Field_ops.Domain.Enums;
using Field_Ops.Application.DTO;


namespace Field_Ops.Application.Contracts.Service
{
   

    public interface IServiceTasksService
    {
        Task<int> CreateAsync(ServiceTaskCreateDto dto);
        Task<IEnumerable<dynamic>> GetAllAsync();
        Task<dynamic?> GetByIdAsync(int id);
        Task<int> UpdateAsync(ServiceTaskUpdateDto dto);
        Task<int> UpdateStatusAsync(ServiceTaskUpdateStatusDto dto);
        Task<int> DeleteAsync(int id, int actionUserId);

        Task<IEnumerable<dynamic>> GetTasksByCustomerAsync(int actionUserId);
        Task<IEnumerable<dynamic>> GetTasksByStatusAsync(ServiceTaskStatus status);
        Task<IEnumerable<dynamic>> GetTasksByTechnicianAsync(int employeeId);
        Task<IEnumerable<dynamic>> GetTasksBySubscriptionIdAsync(int subscriptionId);
    }

}
