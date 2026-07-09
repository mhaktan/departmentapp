using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.Trainings
{
    public class TrainingEntityTests
    {
        [Fact]
        public void Training_ShouldBeCreatable()
        {
            // Act
            var entity = new Training();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void Training_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new Training();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void Training_Title_ShouldAcceptValue()
        {
            var entity = new Training { Title = "Test Value" };
            entity.Title.Should().Be("Test Value");
        }

    }
}
