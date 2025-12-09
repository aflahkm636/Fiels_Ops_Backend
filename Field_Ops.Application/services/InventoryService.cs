using Field_Ops.Application.common;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.InventoryDto;

public class InventoryService : IInventoryService
{
    private readonly IProductsRepository _repo;

    public InventoryService(IProductsRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<int>> CreateAsync(ProductCreateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");
        if (dto.Price <= 0) throw new ArgumentException("Price must be positive.");
        if (dto.QuantityInStock < 0) throw new ArgumentException("Invalid Quantity.");
        if (dto.ReorderLevel < 0) throw new ArgumentException("Invalid ReorderLevel.");
        if (dto.ActionUserId <= 0) throw new ArgumentException("Invalid ActionUserId.");

        int id = await _repo.CreateAsync(dto);

        return ApiResponse<int>.SuccessResponse(200, "Product created successfully.", id);
    }


    public async Task<ApiResponse<bool>> UpdateAsync(ProductUpdateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        if (dto.Id <= 0) throw new ArgumentException("Invalid Id.");
        if (dto.ActionUserId <= 0) throw new ArgumentException("Invalid ActionUserId.");

        int result = await _repo.UpdateAsync(dto);


        return ApiResponse<bool>.SuccessResponse(
            200,
            "Product updated successfully." 
         
        );
    }


    public async Task<ApiResponse<bool>> DeleteAsync(int id, int actionUserId)
    {
        if (id <= 0) throw new ArgumentException("Invalid Id.");
        if (actionUserId <= 0) throw new ArgumentException("Invalid ActionUserId.");

        int result = await _repo.DeleteAsync(id, actionUserId);
        bool success = result > 0;

        return ApiResponse<bool>.SuccessResponse(
            200,
            "Product deleted successfully." 
          
        );
    }

   
    public async Task<ApiResponse<dynamic>> FilterAsync(ProductFilterDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var result = await _repo.FilterAsync(dto);
        return ApiResponse<dynamic>.SuccessResponse(200, "Products fetched successfully.", result);
    }

    public async Task<ApiResponse<dynamic>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return ApiResponse<dynamic>.SuccessResponse(200, "Products fetched.", data);
    }

   
    public async Task<ApiResponse<dynamic>> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Invalid Id.");

        var data = await _repo.GetByIdAsync(id);

        return ApiResponse<dynamic>.SuccessResponse(200, "Product fetched.", data);
    }


    public async Task<ApiResponse<bool>> IncreaseQuantityAsync(int id, int qty, int userId)
    {
        if (id <= 0) throw new ArgumentException("Invalid Id.");
        if (qty <= 0) throw new ArgumentException("Quantity must be positive.");
        if (userId <= 0) throw new ArgumentException("Invalid UserId.");

        int result = await _repo.IncreaseQuantityAsync(id, qty, userId);

        return ApiResponse<bool>.SuccessResponse(
            200,
            "Quantity increased."
         
        );
    }


 
    public async Task<ApiResponse<bool>> DecreaseQuantityAsync(int id, int qty, int userId)
    {
        if (id <= 0) throw new ArgumentException("Invalid Id.");
        if (qty <= 0) throw new ArgumentException("Quantity must be positive.");
        if (userId <= 0) throw new ArgumentException("Invalid UserId.");

        int result = await _repo.DecreaseQuantityAsync(id, qty, userId);

        return ApiResponse<bool>.SuccessResponse(
            200,
            "Quantity decreased."
        );
    }


    public async Task<ApiResponse<dynamic>> LowStockAsync()
    {
        var data = await _repo.GetLowStockAsync();

        return ApiResponse<dynamic>.SuccessResponse(200, "Low stock products fetched.", data);
    }

    public async Task<ApiResponse<decimal>> GetInventoryValueAsync()
    {
        decimal value = await _repo.GetTotalInventoryValueAsync();

        return ApiResponse<decimal>.SuccessResponse(200, "Total inventory value calculated.", value);
    }
}
