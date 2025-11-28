using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Field_Ops.Application.DTO.UserDto;
using Field_Ops.Application.Contracts.Repository;

namespace Field_Ops.Infrastructure.Repository
{
    

    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnection _db;

        public UsersRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> RegisterUserAsync(UserRegisterDto dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", "INSERT");


            parameters.Add("@Name", dto.Name);
            parameters.Add("@Email", dto.Email);
            parameters.Add("@Phone", dto.Phone);
            parameters.Add("@PasswordHash", dto.PasswordHash);
            parameters.Add("@Role", dto.Role);
            parameters.Add("@Status", true);
            parameters.Add("@ProfileImage", null);
            parameters.Add("@CreatedBy", dto.CreatedBy);

            return await _db.ExecuteScalarAsync<int>(
                "SP_USERS",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<dynamic?> GetUserByEmailAsync(string email)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", "GETUSER_BY_EMAIL");
            parameters.Add("@Email", email);

            return await _db.QueryFirstOrDefaultAsync<dynamic>(
                "SP_USERS",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<dynamic>> GetAllAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", "GETALL");

            return await _db.QueryAsync<dynamic>(
                "SP_USERS",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<dynamic?> GetByIdAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FLAG", "GETBYID");
            parameters.Add("@Id", id);

            return await _db.QueryFirstOrDefaultAsync<dynamic>(
                "SP_USERS",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

}
