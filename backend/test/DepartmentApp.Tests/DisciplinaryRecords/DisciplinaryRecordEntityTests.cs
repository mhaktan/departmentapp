using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.DisciplinaryRecords
{
    public class DisciplinaryRecordEntityTests
    {
        [Fact]
        public void DisciplinaryRecord_ShouldBeCreatable()
        {
            // Act
            var entity = new DisciplinaryRecord();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void DisciplinaryRecord_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new DisciplinaryRecord();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.AcknowledgedByEmployee.Should().Be(false);
        }

        [Fact]
        public void DisciplinaryRecord_Description_ShouldAcceptValue()
        {
            var entity = new DisciplinaryRecord { Description = "Test Value" };
            entity.Description.Should().Be("Test Value");
        }

    }
}
