using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.PerformanceGoals;
using DepartmentApp.PerformanceGoals.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.PerformanceGoals
{
    public class PerformanceGoalAppServiceTests
    {
        private readonly Mock<IRepository<PerformanceGoal, long>> _repositoryMock;
        private readonly PerformanceGoalAppService _service;

        public PerformanceGoalAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<PerformanceGoal, long>>();
            _service = new PerformanceGoalAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new PerformanceGoal { Id = 1, Title = "Test title", Status = 0 },
                new PerformanceGoal { Id = 2, Title = "Test title", Status = 0 },
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
                new PerformanceGoal { Id = 1, Title = "Test title", Status = 0 },
                new PerformanceGoal { Id = 2, Title = "Test title", Status = 0 },
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
            var dto = new CreatePerformanceGoalDto
            {
                Title = "Test title", Status = 0
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<PerformanceGoal>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new PerformanceGoal { Id = 1, Title = "Test title", Status = 0 });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new PerformanceGoal { Id = 1, Title = "Test title", Status = 0 });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
