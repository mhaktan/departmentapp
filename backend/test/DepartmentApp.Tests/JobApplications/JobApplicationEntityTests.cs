using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.JobApplications
{
    public class JobApplicationEntityTests
    {
        [Fact]
        public void JobApplication_ShouldBeCreatable()
        {
            // Act
            var entity = new JobApplication();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void JobApplication_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new JobApplication();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void JobApplication_ApplicantFirstName_ShouldAcceptValue()
        {
            var entity = new JobApplication { ApplicantFirstName = "Test Value" };
            entity.ApplicantFirstName.Should().Be("Test Value");
        }

        [Fact]
        public void JobApplication_ApplicantLastName_ShouldAcceptValue()
        {
            var entity = new JobApplication { ApplicantLastName = "Test Value" };
            entity.ApplicantLastName.Should().Be("Test Value");
        }

        [Fact]
        public void JobApplication_ApplicantEmail_ShouldAcceptValue()
        {
            var entity = new JobApplication { ApplicantEmail = "Test Value" };
            entity.ApplicantEmail.Should().Be("Test Value");
        }

    }
}
