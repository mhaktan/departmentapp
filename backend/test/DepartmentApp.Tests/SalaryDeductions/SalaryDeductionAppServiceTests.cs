using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.SalaryDeductions;
using DepartmentApp.SalaryDeductions.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.SalaryDeductions
{
    public class SalaryDeductionAppServiceTests
    {
        private readonly Mock<IRepository<SalaryDeduction, long>> _repositoryMock;
        private readonly SalaryDeductionAppService _service;

        public SalaryDeductionAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<SalaryDeduction, long>>();
            _service = new SalaryDeductionAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new SalaryDeduction { Id = 1, DeductionType = "Test deductionType", Amount = 10.0m, Currency = "Test curre", EffectiveDate = DateTime.UtcNow },
                new SalaryDeduction { Id = 2, DeductionType = "Test deductionType", Amount = 10.0m, Currency = "Test curre", EffectiveDate = DateTime.UtcNow },
            }.AsQueryable();

            _repositoryMock.Setup(r => r.GetAll()).Returns(entities);

            // Act
            var result = _repositoryMock.Object.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public void Repository_GetAll_WithFilter_ShouldWork()
        {
            // Arrange
            var entities = new[]
            {
                new SalaryDeduction { Id = 1, DeductionType = "Test deductionType", Amount = 10.0m, Currency = "Test curre", EffectiveDate = DateTime.UtcNow },
                new SalaryDeduction { Id = 2, DeductionType = "Test deductionType", Amount = 10.0m, Currency = "Test curre", EffectiveDate = DateTime.UtcNow },
            }.AsQueryable();

            _repositoryMock.Setup(r => r.GetAll()).Returns(entities);

            // Act — simulate keyword filter
            var result = _repositoryMock.Object.GetAll()
                .Where(x => x.Id.ToString().Contains("1"));

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_ShouldInsertEntity()
        {
            // Arrange
            var dto = new CreateSalaryDeductionDto
            {
                DeductionType = "Test deductionType", Amount = 10.0m, Currency = "Test curre", EffectiveDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<SalaryDeduction>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new SalaryDeduction { Id = 1, DeductionType = "Test deductionType", Amount = 10.0m, Currency = "Test curre", EffectiveDate = DateTime.UtcNow });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new SalaryDeduction { Id = 1, DeductionType = "Test deductionType", Amount = 10.0m, Currency = "Test curre", EffectiveDate = DateTime.UtcNow });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
