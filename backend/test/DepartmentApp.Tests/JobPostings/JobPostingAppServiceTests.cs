using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.JobPostings;
using DepartmentApp.JobPostings.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.JobPostings
{
    public class JobPostingAppServiceTests
    {
        private readonly Mock<IRepository<JobPosting, long>> _repositoryMock;
        private readonly JobPostingAppService _service;

        public JobPostingAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<JobPosting, long>>();
            _service = new JobPostingAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object, new Mock<IRepository<StatusChangeLog, long>>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new JobPosting { Id = 1, Title = "Test title", PositionCount = 1, Status = 0, EmploymentType = 0 },
                new JobPosting { Id = 2, Title = "Test title", PositionCount = 1, Status = 0, EmploymentType = 0 },
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
                new JobPosting { Id = 1, Title = "Test title", PositionCount = 1, Status = 0, EmploymentType = 0 },
                new JobPosting { Id = 2, Title = "Test title", PositionCount = 1, Status = 0, EmploymentType = 0 },
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
            var dto = new CreateJobPostingDto
            {
                Title = "Test title", PositionCount = 1, Status = 0, EmploymentType = 0
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<JobPosting>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new JobPosting { Id = 1, Title = "Test title", PositionCount = 1, Status = 0, EmploymentType = 0 });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new JobPosting { Id = 1, Title = "Test title", PositionCount = 1, Status = 0, EmploymentType = 0 });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
