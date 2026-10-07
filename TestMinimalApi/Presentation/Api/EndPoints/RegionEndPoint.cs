using TestMinimalApi.Application.DTOs;
using TestMinimalApi.Application.Interfaces.Repositories;
using TestMinimalApi.Application.Interfaces.Services;

namespace TestMinimalApi.Presentation.Api.EndPoints
{
    public static class RegionEndPoint
    {
        
        public static void MapTestEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/tesminimalapi").
                       //RequireAuthorization().
                       WithTags("Region");
            group.MapGet("/getallregions", GetAllAsync)
                .WithName("GetAllRegions");
            //group.MapPost("/createregion", Create)
            //    .WithName("CreateRegion");
            //group.MapPut("/updateregion", Update)
            //   .WithName("UpdateRegion");
            group.MapDelete("/deleteregion/{id:int}",Delete )
               .WithName("DeleteRegion");
        }
        private static async Task<IResult> GetAllAsync(
           IRegionService service)
        {
            var result = await service.GetAllAsync();
            return Results.Ok(result);
        }
        //private static async Task<IResult> GetById(int id,
        //  IRegionRepository testRepository)
        //{
        //    var result = id;
        //    return Results.Ok(result);
        //}
        //private static async Task<IResult> Create(Region obj,
        // IRegionService service)
        //{
        //   var response= await service.CreateAsync(obj);
        //    return Results.Ok(response);
        //}
        //private static async Task<IResult> Update(Region obj,
        //IRegionService service)
        //{
        //    var response = await service.UpdateAsync(obj);
        //    return Results.Ok(response);
        //}
        private static async Task<IResult> Delete(int id,
       IRegionService service)
        {
            var response = await service.DeleteAsync(id);
            return Results.Ok(response);
        }
    }
}
