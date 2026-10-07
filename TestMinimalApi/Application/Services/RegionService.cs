using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;
using TestMinimalApi.Application.DTOs;
using TestMinimalApi.Application.Interfaces.Repositories;
using TestMinimalApi.Application.Interfaces.Services;
using TestMinimalApi.Domain.Entities;
using TestMinimalApi.Presentation.CoreModels;

namespace TestMinimalApi.Application.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepo;
        private readonly IUnitOfWork _uow;
        private readonly IValidator<Region> _validator;
        public RegionService(IRegionRepository regionReop, IUnitOfWork uow, IValidator<Region> validator)
        {
            _regionRepo = regionReop;
            _uow = uow;
            _validator = validator;
        }
        public async Task<List<StateRegion>> GetAllAsync()
        {
            var result = await _regionRepo.GetAllAsync();
            Log.Information("Fetched all regions, count: {Count}", result.Count);
            return result;
        }
        //public async Task<ResponseModel> CreateAsync(Region info)
        //{
        //    var validationResult = await _validator.ValidateAsync(info);
        //    if (!validationResult.IsValid)
        //    {
        //        Log.Warning("Invalid model state: {@Errors}", validationResult);
        //        return new ResponseModel
        //        {
        //            IsSuccess = false,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest, // Bad Request
        //            Message = "Invalid request data."
        //        };
               
        //    }
        //    await _uow.BeginTransactionAsync();
        //    try
        //    {
        //        //check duplicated name name mm in db
        //        if (await _regionRepo.ExistsAsync(info.Name, info.NameMM))
        //        {
        //            Log.Warning("Region with Name {RegionName} and Name(MM) {RegionNameMM} already exists", info.Name, info.NameMM);
        //            return new ResponseModel
        //            {
        //                IsSuccess = false,
        //                StatusCode = System.Net.HttpStatusCode.BadRequest, // Bad Request
        //                Message = "Region with the same name already exists."
        //            };
        //        }
        //        //Add New Region
        //        var entity = new StateRegion
        //        {
        //            Code = info.Code,
        //            GLISRefCode = info.GLISRefCode,
        //            Name = info.Name,
        //            NameMM = info.NameMM,
        //            SystemUse = "Y"
        //        };


        //        await _regionRepo.CreateAsync(entity);
        //        await _uow.CommitAsync();
        //        Log.Information("Region {RegionName} created with Code {RegionCode}", entity.Name, entity.Code);
        //        return new ResponseModel
        //        {
        //            IsSuccess = true,
        //            StatusCode = System.Net.HttpStatusCode.Created, // Created
        //            Message = "Region successfully saved!"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        await _uow.RollbackAsync();
        //        Log.Error(ex, "Error occurred while processing Region: {RegionCode} - {RegionName}", info.Code, info.Name);
        //        return new ResponseModel
        //        {
        //            IsSuccess = false,
        //            StatusCode = System.Net.HttpStatusCode.InternalServerError, // Internal Server Error
        //            Message = "Error in processing"
        //        };
        //    }
        //}

        //public async Task<ResponseModel> UpdateAsync(Region info)
        //{
        //    var validationResult = await _validator.ValidateAsync(info);
        //    if (!validationResult.IsValid)
        //    {
        //        Log.Warning("Invalid model state: {@Errors}", validationResult);
        //        return new ResponseModel
        //        {
        //            IsSuccess = false,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest, // Bad Request
        //            Message = "Invalid request data."
        //        };

        //    }
        //    await _uow.BeginTransactionAsync();
        //    try
        //    {
        //        //check duplicated name name mm in db
        //        var existingRegion = await _regionRepo.GetByIdAsync(info.Id ?? 0);
        //        if (existingRegion==null)
        //        {
        //            Log.Warning("Region with ID {RegionId} not exists", info.Id);
        //            return new ResponseModel
        //            {
        //                IsSuccess = false,
        //                StatusCode = System.Net.HttpStatusCode.NotFound, // Bad Request
        //                Message = "Region not exists."
        //            };
        //        }
        //        //Add New Region
               
        //            existingRegion.GLISRefCode = info.GLISRefCode;
        //           existingRegion.Name = info.Name;
        //            existingRegion.NameMM = info.NameMM;
                


        //        await _regionRepo.UpdateAsync(existingRegion);
        //        await _uow.CommitAsync();
        //        Log.Information("Region {RegionId} updated", existingRegion.Id);
        //        return new ResponseModel
        //        {
        //            IsSuccess = true,
        //            StatusCode = System.Net.HttpStatusCode.OK, // Updated
        //            Message = "Region successfully updated!"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        await _uow.RollbackAsync();
        //        Log.Error(ex, "Error occurred while processing Region Id: {RegionId}", info.Id);
        //        return new ResponseModel
        //        {
        //            IsSuccess = false,
        //            StatusCode = System.Net.HttpStatusCode.InternalServerError, // Internal Server Error
        //            Message = "Error in processing"
        //        };
        //    }
        //}

        public async Task<ResponseModel> DeleteAsync(int id)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                //get existing region by id 
                var existingRegion = await _regionRepo.GetByIdAsync(id);
                if (existingRegion == null)
                {
                    Log.Warning("Region with ID {RegionId} not exists", id);
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = System.Net.HttpStatusCode.NotFound, // Bad Request
                        Message = "Region not exists."
                    };
                }
                
                //delete existing region by id
                await _regionRepo.DeleteAsync(existingRegion);
                await _uow.CommitAsync();
                Log.Information("Region {RegionId} deleted", id);
                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK, // Updated
                    Message = "Region successfully deleted!"
                };
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                Log.Error(ex, "Error occurred while processing Region Id: {RegionId}", id);
                return new ResponseModel
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError, // Internal Server Error
                    Message = "Error in processing"
                };
            }
        }

    }
}
