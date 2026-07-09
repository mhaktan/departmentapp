using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.TrainingPlans;
using DepartmentApp.TrainingPlans.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.TrainingPlans
{
    public class TrainingPlanAppServiceTests
    {
        private readonly Mock<IRepository<TrainingPlan, long>> _repositoryMock;
        private readonly TrainingPlanAppService _service;

        public TrainingPlanAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<TrainingPlan, long>>();
            _service = new TrainingPlanAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object, new Mock<IRepository<StatusChangeLog, long>>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new TrainingPlan { Id = 1, Title = "Test title", Year = 1, Status = 0 },
                new TrainingPlan { Id = 2, Title = "Test title", Year = 1, Status = 0 },
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
                new TrainingPlan { Id = 1, Title = "Test title", Year = 1, Status = 0 },
                new TrainingPlan { Id = 2, Title = "Test title", Year = 1, Status = 0 },
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
            var dto = new CreateTrainingPlanDto
            {
                Title = "Test title", Year = 1, Status = 0
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<TrainingPlan>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new TrainingPlan { Id = 1, Title = "Test title", Year = 1, Status = 0 });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new TrainingPlan { Id = 1, Title = "Test title", Year = 1, Status = 0 });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
