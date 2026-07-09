using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.OvertimeRecords
{
    public class OvertimeRecordEntityTests
    {
        [Fact]
        public void OvertimeRecord_ShouldBeCreatable()
        {
            // Act
            var entity = new OvertimeRecord();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void OvertimeRecord_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new OvertimeRecord();

            // Assert
            entity.Id.Should().Be(default(long));

        }


    }
}
