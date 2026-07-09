using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.LeaveTypes
{
    public class LeaveTypeEntityTests
    {
        [Fact]
        public void LeaveType_ShouldBeCreatable()
        {
            // Act
            var entity = new LeaveType();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void LeaveType_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new LeaveType();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.RequiresHRApproval.Should().Be(false);
            entity.IsPaid.Should().Be(false);
        }

        [Fact]
        public void LeaveType_Name_ShouldAcceptValue()
        {
            var entity = new LeaveType { Name = "Test Value" };
            entity.Name.Should().Be("Test Value");
        }

        [Fact]
        public void LeaveType_Code_ShouldAcceptValue()
        {
            var entity = new LeaveType { Code = "Test Value" };
            entity.Code.Should().Be("Test Value");
        }

    }
}
