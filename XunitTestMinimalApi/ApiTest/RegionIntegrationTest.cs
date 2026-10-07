using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Net;
using System.Net.Http.Json;
using TestMinimalApi.Application.DTOs;
using TestMinimalApi.Application.Interfaces.Repositories;
using TestMinimalApi.Application.Services;
using TestMinimalApi.Application.Validators;
using TestMinimalApi.Domain.Entities;
using TestMinimalApi.Infrastructure;
using TestMinimalApi.Infrastructure.Data;
using TestMinimalApi.Services.Repositories;
using XunitTestMinimalApi.Utilities;

namespace XunitTestMinimalApi
{
    public class RegionIntegrationTest
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnRegions_FromInMemoryDb()
        {
            // Arrange
            using var context = DbContextHelper.CreateDbContext();

            // ၁။ အရင် Test တွေက ကျန်ခဲ့တဲ့ Data များကို ဖယ်ရှားရန် (သို့မဟုတ် Database ကို reset လုပ်ရန်)
            context.StateRegion.RemoveRange(context.StateRegion);
            await context.SaveChangesAsync();

            // ၂။ Test အတွက် လိုအပ်သော Data အသစ်များ ထည့်ခြင်း
            context.StateRegion.AddRange(
                new StateRegion { Id = 2, Code = "YGN001", Name = "Yangon", NameMM = "ရန်ကုန်" },
                new StateRegion { Id = 3, Code = "MDY001", Name = "Mandalay", NameMM = "မန္တလေး" }
            );
            await context.SaveChangesAsync();

            var repo = new RegionRepository(context);
            var uow = new UnitOfWork(context);
            var validator = new RegionValidator();
            var service = new RegionService(repo, uow, validator);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2); // အခုဆိုရင် ဒေတာ ၂ ခုတည်းသာ ရှိတော့မည်ဖြစ်၍ အောင်မြင်ပါမည်
            result[0].Name.Should().Be("Yangon");
            result[1].Name.Should().Be("Mandalay");
        }

        [Fact]
        public async Task CreateAsync_ShouldSaveRegion_WhenValid()
        {
            using var context = DbContextHelper.CreateDbContext();
            context.StateRegion.RemoveRange(context.StateRegion); 
            await context.SaveChangesAsync();
            var repo = new RegionRepository(context);
            var uow = new UnitOfWork(context);
            var validator = new RegionValidator();
            var service = new RegionService(repo, uow, validator);
            var region = new Region
            {
                Code = "YGN2992",
                Name = "Yangon1",
                NameMM = "ရန်ကုန်1",
                GLISRefCode = "GLIS001",
                SystemUse = "Y"
            };
            var result = await service.CreateAsync(region);
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }


        [Fact]
        public async Task UpdateAsync_ShouldModifyRegion_WhenValid()
        {
            using var context = DbContextHelper.CreateDbContext();
            context.StateRegion.RemoveRange(context.StateRegion);
            await context.SaveChangesAsync();
            var repo = new RegionRepository(context);
            var uow = new UnitOfWork(context);
            var validator = new RegionValidator();
            var service = new RegionService(repo, uow, validator);

            // Arrange: create initial region
            var region = new Region
            {
                Id = 1,
                Code = "YGN2992",
                Name = "Yangon1",
                NameMM = "ရန်ကုန်1",
                GLISRefCode = "GLIS001",
                SystemUse = "Y"
            };
            await service.CreateAsync(region);

            // Act: update the region
            region.Name = "Yangon Updated";
            var result = await service.UpdateAsync(region);

            // Assert: check success
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify persisted change
            var updated = await repo.GetByIdAsync(region.Id ?? 0);
            updated.Name.Should().Be("Yangon Updated");
        }
        [Fact]
        public async Task DeleteAsync_ShouldRemoveRegion_WhenValid()
        {
            using var context = DbContextHelper.CreateDbContext();
            context.StateRegion.RemoveRange(context.StateRegion);
            await context.SaveChangesAsync();

            var repo = new RegionRepository(context);
            var uow = new UnitOfWork(context);
            var validator = new RegionValidator();
            var service = new RegionService(repo, uow, validator);

            // Arrange: create a region first (Id ကို manual မထည့်ဘဲ DB/Repository ကိုယ်တိုင် Auto Generate လုပ်ခွင့်ပေးခြင်း)
            var region = new Region
            {
                Code = "YGN2992",
                Name = "Yangon1",
                NameMM = "ရန်ကုန်1",
                GLISRefCode = "GLIS001",
                SystemUse = "Y"
            };

            var createResult = await service.CreateAsync(region);
            createResult.IsSuccess.Should().BeTrue();

            // Database ထဲမှာ အမှန်တကယ် Save ဖြစ်သွားတဲ့ Region ကို Code ဖြင့် ရှာပြီး ID ကို ရယူခြင်း
            var savedRegions = await repo.GetAllAsync();
            var createdRegion = savedRegions.FirstOrDefault(x => x.Code == "YGN2992");
            createdRegion.Should().NotBeNull();
            int regionId = createdRegion!.Id;

            // Act: delete the region using the correct generated ID
            var result = await service.DeleteAsync(regionId);

            // Assert: check success
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify removed data in db 
            var deleted = await repo.GetByIdAsync(regionId);
            deleted.Should().BeNull();
        }
    }
}
