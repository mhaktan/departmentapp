using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.LeaveRequests
{
    public class LeaveRequestEntityTests
    {
        [Fact]
        public void LeaveRequest_ShouldBeCreatable()
        {
            // Act
            var entity = new LeaveRequest();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void LeaveRequest_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new LeaveRequest();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.RequiresHRApproval.Should().Be(false);
            entity.BalanceDeducted.Should().Be(false);
        }


    }
}
