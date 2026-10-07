using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestMinimalApi.Presentation.CoreModels;
using TestMinimalApi.Application;
using TestMinimalApi.Application.DTOs;
using TestMinimalApi.Application.Interfaces.Repositories;
using TestMinimalApi.Application.Interfaces.Services;
using TestMinimalApi.Application.Services;
using TestMinimalApi.Domain.Entities;
using FluentValidation;

namespace XunitTestMinimalApi.ApiTest
{
    public class RegionUnitTest
    {
        private readonly Mock<IRegionRepository> _repoMock;
        private readonly Mock<IRegionService> _serviceMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<Region>> _validatorMock;
        private readonly RegionService _service;
        public RegionUnitTest()
        {
            _serviceMock = new Mock<IRegionService>();
            _repoMock = new Mock<IRegionRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<Region>>();
            _service = new RegionService(_repoMock.Object, _uowMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRegions()
        {
            // Arrange
            var regions = new List<StateRegion>
    {
        new StateRegion { Id = 1, Name = "Yangon" },
        new StateRegion { Id = 2, Name = "Mandalay" }
    };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(regions);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Yangon");
            result[1].Name.Should().Be("Mandalay");

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreated_WhenValid()
        {
            // Arrange
            var info = new Region
            {
                Code = "YGN001",
                Name = "Yangon",
                NameMM = "ရန်ကုန်",
                GLISRefCode = "GLIS001"
            };

            _uowMock.Setup(u => u.BeginTransactionAsync())
                    .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.ExistsAsync(info.Name, info.NameMM))
                     .ReturnsAsync(false);

            _repoMock.Setup(r => r.CreateAsync(It.IsAny<StateRegion>()))
                     .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CommitAsync())
                    .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(info);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Message.Should().Be("Region successfully saved!");

            _uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _repoMock.Verify(r => r.ExistsAsync(info.Name, info.NameMM), Times.Once);
            _repoMock.Verify(r => r.CreateAsync(It.Is<StateRegion>(
                x => x.Name == info.Name && x.NameMM == info.NameMM
            )), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(), Times.Once);
            _uowMock.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_WhenRegionExists()
        {
            // Arrange
            var region = new Region
            {
                Id = 1,
                Code = "YGN001",
                Name = "Yangon Updated",
                NameMM = "ရန်ကုန်",
                GLISRefCode = "GLIS001"
            };
            var existingEntity = new StateRegion
            {
                Id = 1,
                Name = "Yangon",
                NameMM = "ရန်ကုန်",
                GLISRefCode = "GLIS001"
            };

            _repoMock.Setup(r => r.GetByIdAsync(existingEntity.Id))
                     .ReturnsAsync(existingEntity);

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<StateRegion>())).Returns(Task.CompletedTask);
               

            // Act
            var result = await _service.UpdateAsync(region);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            _repoMock.Verify(r => r.UpdateAsync(existingEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_WhenRegionExists()
        {
            // Arrange
            var region = new StateRegion { Id = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(region.Id))
                     .ReturnsAsync(region);

            _repoMock.Setup(r => r.DeleteAsync(region));


            // Act
            var result = await _service.DeleteAsync(region.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            _repoMock.Verify(r => r.DeleteAsync(region), Times.Once);
        }

    }
}
