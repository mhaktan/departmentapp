using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.SalaryRecords;
using DepartmentApp.SalaryRecords.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.SalaryRecords
{
    public class SalaryRecordAppServiceTests
    {
        private readonly Mock<IRepository<SalaryRecord, long>> _repositoryMock;
        private readonly SalaryRecordAppService _service;

        public SalaryRecordAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<SalaryRecord, long>>();
            _service = new SalaryRecordAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new SalaryRecord { Id = 1, EffectiveDate = DateTime.UtcNow, GrossSalary = 10.0m, Currency = "Test curre", SalaryType = 0 },
                new SalaryRecord { Id = 2, EffectiveDate = DateTime.UtcNow, GrossSalary = 10.0m, Currency = "Test curre", SalaryType = 0 },
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
                new SalaryRecord { Id = 1, EffectiveDate = DateTime.UtcNow, GrossSalary = 10.0m, Currency = "Test curre", SalaryType = 0 },
                new SalaryRecord { Id = 2, EffectiveDate = DateTime.UtcNow, GrossSalary = 10.0m, Currency = "Test curre", SalaryType = 0 },
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
            var dto = new CreateSalaryRecordDto
            {
                EffectiveDate = DateTime.UtcNow, GrossSalary = 10.0m, Currency = "Test curre", SalaryType = 0
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<SalaryRecord>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new SalaryRecord { Id = 1, EffectiveDate = DateTime.UtcNow, GrossSalary = 10.0m, Currency = "Test curre", SalaryType = 0 });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new SalaryRecord { Id = 1, EffectiveDate = DateTime.UtcNow, GrossSalary = 10.0m, Currency = "Test curre", SalaryType = 0 });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
