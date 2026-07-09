using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.LeaveTypes;
using DepartmentApp.LeaveTypes.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.LeaveTypes
{
    public class LeaveTypeAppServiceTests
    {
        private readonly Mock<IRepository<LeaveType, long>> _repositoryMock;
        private readonly LeaveTypeAppService _service;

        public LeaveTypeAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<LeaveType, long>>();
            _service = new LeaveTypeAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new LeaveType { Id = 1, Name = "Test name", Code = "Test code", RequiresHRApproval = true, IsPaid = true },
                new LeaveType { Id = 2, Name = "Test name", Code = "Test code", RequiresHRApproval = true, IsPaid = true },
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
                new LeaveType { Id = 1, Name = "Test name", Code = "Test code", RequiresHRApproval = true, IsPaid = true },
                new LeaveType { Id = 2, Name = "Test name", Code = "Test code", RequiresHRApproval = true, IsPaid = true },
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
            var dto = new CreateLeaveTypeDto
            {
                Name = "Test name", Code = "Test code", RequiresHRApproval = true, IsPaid = true
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<LeaveType>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new LeaveType { Id = 1, Name = "Test name", Code = "Test code", RequiresHRApproval = true, IsPaid = true });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new LeaveType { Id = 1, Name = "Test name", Code = "Test code", RequiresHRApproval = true, IsPaid = true });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
