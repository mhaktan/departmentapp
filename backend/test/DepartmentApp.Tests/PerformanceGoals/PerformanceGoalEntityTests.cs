using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.PerformanceGoals
{
    public class PerformanceGoalEntityTests
    {
        [Fact]
        public void PerformanceGoal_ShouldBeCreatable()
        {
            // Act
            var entity = new PerformanceGoal();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void PerformanceGoal_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new PerformanceGoal();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void PerformanceGoal_Title_ShouldAcceptValue()
        {
            var entity = new PerformanceGoal { Title = "Test Value" };
            entity.Title.Should().Be("Test Value");
        }

    }
}
