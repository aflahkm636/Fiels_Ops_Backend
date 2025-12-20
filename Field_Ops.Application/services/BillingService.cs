using Field_Ops.Application.common;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.BIllingDto;

namespace Field_Ops.Application.Service
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _repo;

        public BillingService(IBillingRepository repo)
        {
            _repo = repo;
        }


        public async Task<ApiResponse<IEnumerable<dynamic>>> GetPendingAsync()
        {
            var bills = await _repo.GetPendingAsync();

            return ApiResponse<IEnumerable<dynamic>>.SuccessResponse(
                200,
                "Pending bills fetched successfully.",
                bills
            );
        }


        public async Task<ApiResponse<dynamic?>> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Bill Id.");

            var bill = await _repo.GetByIdAsync(id);

            return ApiResponse<dynamic?>.SuccessResponse(
                200,
                bill != null ? "Bill fetched successfully." : "Bill not found.",
                bill
            );
        }


        public async Task<ApiResponse<IEnumerable<dynamic>>> GetByCustomerAsync(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Invalid Customer Id.");

            var bills = await _repo.GetByCustomerAsync(customerId);

            return ApiResponse<IEnumerable<dynamic>>.SuccessResponse(
                200,
                "Customer bills fetched successfully.",
                bills
            );
        }


        public async Task<ApiResponse<BillingDto>> UpdateDiscountAsync( BillingDiscountUpdateDto dto)
        {
            if (dto.BillingId <= 0)
                throw new ArgumentException("Invalid Bill Id.");

            if (dto.DiscountPercent < 0 || dto.DiscountPercent > 100)
                throw new ArgumentException("Invalid discount percent.");

            var bill = await _repo.UpdateDiscountAsync(dto);

            return ApiResponse<BillingDto>.SuccessResponse(
                200,
                "Discount updated successfully.",
                bill
            );
        }



        public async Task<ApiResponse<BillingDto>> FinalizeAsync( int billId,int actionUserId)
        {
            if (billId <= 0)
                throw new ArgumentException("Invalid Bill Id.");

            var bill = await _repo.FinalizeAsync(billId, actionUserId);

            return ApiResponse<BillingDto>.SuccessResponse(
                200,
                "Bill finalized successfully.",
                bill
            );
        }


        public async Task<ApiResponse<bool>> RegenerateAsync(int billId, int actionUserId)
        {
            if (billId <= 0)
                throw new ArgumentException("Invalid Bill Id.");

            await _repo.RegenerateAsync(billId, actionUserId);

            return ApiResponse<bool>.SuccessResponse(
                200,
                "Bill regenerated successfully.",
                true
            );
        }

    }
}
