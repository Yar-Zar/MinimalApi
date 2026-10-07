

using Microsoft.EntityFrameworkCore;
using TestMinimalApi.Presentation.CoreModels;
using TestMinimalApi.Application.DTOs;
using TestMinimalApi.Application.Interfaces.Repositories;
using TestMinimalApi.Domain.Entities;
using TestMinimalApi.Infrastructure.Data;

namespace TestMinimalApi.Services.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly AppDbContext _dbContext;
        public RegionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<StateRegion>> GetAllAsync()
        {
            var result = await _dbContext.StateRegion.ToListAsync();
            return result;
        }
        public async Task CreateAsync(StateRegion entity)
        {
            await _dbContext.StateRegion.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(StateRegion entity)
        {
             _dbContext.StateRegion.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(StateRegion entity)
        {
            _dbContext.StateRegion.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<StateRegion?> GetByIdAsync(int id)
        {
            return await _dbContext.StateRegion.FindAsync(id);
        }
        public async Task<bool> ExistsAsync(string name, string? nameMM)
        {
            return await _dbContext.StateRegion.AnyAsync(r => r.Name == name || r.NameMM == nameMM);
        }
    }
}
