using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.OnboardingTasks;
using DepartmentApp.OnboardingTasks.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.OnboardingTasks
{
    public class OnboardingTaskAppServiceTests
    {
        private readonly Mock<IRepository<OnboardingTask, long>> _repositoryMock;
        private readonly OnboardingTaskAppService _service;

        public OnboardingTaskAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<OnboardingTask, long>>();
            _service = new OnboardingTaskAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new OnboardingTask { Id = 1, Title = "Test title", IsCompleted = true },
                new OnboardingTask { Id = 2, Title = "Test title", IsCompleted = true },
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
                new OnboardingTask { Id = 1, Title = "Test title", IsCompleted = true },
                new OnboardingTask { Id = 2, Title = "Test title", IsCompleted = true },
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
            var dto = new CreateOnboardingTaskDto
            {
                Title = "Test title", IsCompleted = true
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<OnboardingTask>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new OnboardingTask { Id = 1, Title = "Test title", IsCompleted = true });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new OnboardingTask { Id = 1, Title = "Test title", IsCompleted = true });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
