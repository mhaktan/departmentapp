using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.Employees
{
    public class EmployeeEntityTests
    {
        [Fact]
        public void Employee_ShouldBeCreatable()
        {
            // Act
            var entity = new Employee();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void Employee_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new Employee();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void Employee_EmployeeNumber_ShouldAcceptValue()
        {
            var entity = new Employee { EmployeeNumber = "Test Value" };
            entity.EmployeeNumber.Should().Be("Test Value");
        }

        [Fact]
        public void Employee_FirstName_ShouldAcceptValue()
        {
            var entity = new Employee { FirstName = "Test Value" };
            entity.FirstName.Should().Be("Test Value");
        }

        [Fact]
        public void Employee_LastName_ShouldAcceptValue()
        {
            var entity = new Employee { LastName = "Test Value" };
            entity.LastName.Should().Be("Test Value");
        }

        [Fact]
        public void Employee_Email_ShouldAcceptValue()
        {
            var entity = new Employee { Email = "Test Value" };
            entity.Email.Should().Be("Test Value");
        }

    }
}
