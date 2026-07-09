using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.Onboardings
{
    public class OnboardingEntityTests
    {
        [Fact]
        public void Onboarding_ShouldBeCreatable()
        {
            // Act
            var entity = new Onboarding();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void Onboarding_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new Onboarding();

            // Assert
            entity.Id.Should().Be(default(long));

        }


    }
}
