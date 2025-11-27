using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.DTO.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public Task RegisterAsync(CustomerRegisterDto dto)
            => _repo.AddCustomerAsync(dto);

        public Task<IEnumerable<CustomerDto>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<CustomerDto?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public Task<CustomerDto?> GetByUserIdAsync(int userId)
            => _repo.GetByUserIdAsync(userId);
    }

}
