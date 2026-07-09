using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.OnboardingTasks
{
    public class OnboardingTaskEntityTests
    {
        [Fact]
        public void OnboardingTask_ShouldBeCreatable()
        {
            // Act
            var entity = new OnboardingTask();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void OnboardingTask_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new OnboardingTask();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.IsCompleted.Should().Be(false);
        }

        [Fact]
        public void OnboardingTask_Title_ShouldAcceptValue()
        {
            var entity = new OnboardingTask { Title = "Test Value" };
            entity.Title.Should().Be("Test Value");
        }

    }
}
