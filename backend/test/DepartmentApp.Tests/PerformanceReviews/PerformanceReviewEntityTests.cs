using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.PerformanceReviews
{
    public class PerformanceReviewEntityTests
    {
        [Fact]
        public void PerformanceReview_ShouldBeCreatable()
        {
            // Act
            var entity = new PerformanceReview();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void PerformanceReview_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new PerformanceReview();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void PerformanceReview_ReviewPeriod_ShouldAcceptValue()
        {
            var entity = new PerformanceReview { ReviewPeriod = "Test Value" };
            entity.ReviewPeriod.Should().Be("Test Value");
        }

    }
}
