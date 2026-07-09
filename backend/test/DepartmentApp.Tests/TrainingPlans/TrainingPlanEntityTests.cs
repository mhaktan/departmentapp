using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.TrainingPlans
{
    public class TrainingPlanEntityTests
    {
        [Fact]
        public void TrainingPlan_ShouldBeCreatable()
        {
            // Act
            var entity = new TrainingPlan();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void TrainingPlan_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new TrainingPlan();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void TrainingPlan_Title_ShouldAcceptValue()
        {
            var entity = new TrainingPlan { Title = "Test Value" };
            entity.Title.Should().Be("Test Value");
        }

    }
}
