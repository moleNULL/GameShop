namespace WebAPI.Services.Interfaces
{
    public interface ICatalogItemService
    {
        Task<int?> AddAsync(string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId);
        Task<bool> RemoveAsync(int id);
        Task<bool> UpdateAsync(int id, string name, decimal price, int year, string pictureFileName, int availableStock, int companyId, int genreId);
    }
}
