using TestMinimalApi.Application.DTOs;
using TestMinimalApi.Domain.Entities;
using TestMinimalApi.Presentation.CoreModels;

namespace TestMinimalApi.Application.Interfaces.Services
{
    public interface IRegionService
    {
        public Task<List<StateRegion>> GetAllAsync();
        //public Task<ResponseModel> CreateAsync(Region info);
        //public Task<ResponseModel> UpdateAsync(Region info);
        public Task<ResponseModel> DeleteAsync(int id);
    }
}
