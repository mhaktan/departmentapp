using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.JobPostings
{
    public class JobPostingEntityTests
    {
        [Fact]
        public void JobPosting_ShouldBeCreatable()
        {
            // Act
            var entity = new JobPosting();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void JobPosting_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new JobPosting();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void JobPosting_Title_ShouldAcceptValue()
        {
            var entity = new JobPosting { Title = "Test Value" };
            entity.Title.Should().Be("Test Value");
        }

    }
}
