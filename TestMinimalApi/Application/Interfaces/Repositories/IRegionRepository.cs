using TestMinimalApi.Presentation.CoreModels;
using TestMinimalApi.Application.DTOs;
using TestMinimalApi.Domain.Entities;

namespace TestMinimalApi.Application.Interfaces.Repositories
{
    public interface IRegionRepository
    {
        public Task<List<StateRegion>> GetAllAsync();
        public Task CreateAsync(StateRegion info);
        public Task UpdateAsync(StateRegion entity);
        public Task DeleteAsync(StateRegion entity);
        public Task<bool> ExistsAsync(string name, string? nameMM);
        public Task<StateRegion?> GetByIdAsync(int id);
    }
}
