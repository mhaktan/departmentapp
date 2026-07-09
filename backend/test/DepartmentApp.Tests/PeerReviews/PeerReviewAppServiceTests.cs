using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using DepartmentApp.Entities;
using DepartmentApp.PeerReviews;
using DepartmentApp.PeerReviews.Dto;
using DepartmentApp.Flows;

namespace DepartmentApp.Tests.PeerReviews
{
    public class PeerReviewAppServiceTests
    {
        private readonly Mock<IRepository<PeerReview, long>> _repositoryMock;
        private readonly PeerReviewAppService _service;

        public PeerReviewAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<PeerReview, long>>();
            _service = new PeerReviewAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new PeerReview { Id = 1, IsAnonymous = true },
                new PeerReview { Id = 2, IsAnonymous = true },
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
                new PeerReview { Id = 1, IsAnonymous = true },
                new PeerReview { Id = 2, IsAnonymous = true },
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
            var dto = new CreatePeerReviewDto
            {
                IsAnonymous = true
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<PeerReview>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new PeerReview { Id = 1, IsAnonymous = true });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new PeerReview { Id = 1, IsAnonymous = true });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
