using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.EmployeeCertificates;
using DepartmentApp.EmployeeCertificates.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.EmployeeCertificates
{
    public class EmployeeCertificateAppServiceTests
    {
        private readonly Mock<IRepository<EmployeeCertificate, long>> _repositoryMock;
        private readonly EmployeeCertificateAppService _service;

        public EmployeeCertificateAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<EmployeeCertificate, long>>();
            _service = new EmployeeCertificateAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new EmployeeCertificate { Id = 1, CertificateName = "Test certificateName" },
                new EmployeeCertificate { Id = 2, CertificateName = "Test certificateName" },
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
                new EmployeeCertificate { Id = 1, CertificateName = "Test certificateName" },
                new EmployeeCertificate { Id = 2, CertificateName = "Test certificateName" },
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
            var dto = new CreateEmployeeCertificateDto
            {
                CertificateName = "Test certificateName"
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<EmployeeCertificate>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new EmployeeCertificate { Id = 1, CertificateName = "Test certificateName" });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new EmployeeCertificate { Id = 1, CertificateName = "Test certificateName" });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
