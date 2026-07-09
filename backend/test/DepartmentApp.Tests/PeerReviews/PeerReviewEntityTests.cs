using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.PeerReviews
{
    public class PeerReviewEntityTests
    {
        [Fact]
        public void PeerReview_ShouldBeCreatable()
        {
            // Act
            var entity = new PeerReview();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void PeerReview_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new PeerReview();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.IsAnonymous.Should().Be(false);
        }


    }
}
