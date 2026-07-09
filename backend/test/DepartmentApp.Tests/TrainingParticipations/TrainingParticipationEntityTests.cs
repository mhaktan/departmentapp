using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.TrainingParticipations
{
    public class TrainingParticipationEntityTests
    {
        [Fact]
        public void TrainingParticipation_ShouldBeCreatable()
        {
            // Act
            var entity = new TrainingParticipation();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void TrainingParticipation_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new TrainingParticipation();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.Attended.Should().Be(false);
            entity.CertificateEarned.Should().Be(false);
        }


    }
}
