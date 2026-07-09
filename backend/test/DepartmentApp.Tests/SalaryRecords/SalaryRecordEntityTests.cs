using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.SalaryRecords
{
    public class SalaryRecordEntityTests
    {
        [Fact]
        public void SalaryRecord_ShouldBeCreatable()
        {
            // Act
            var entity = new SalaryRecord();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void SalaryRecord_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new SalaryRecord();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void SalaryRecord_Currency_ShouldAcceptValue()
        {
            var entity = new SalaryRecord { Currency = "Test Value" };
            entity.Currency.Should().Be("Test Value");
        }

    }
}
