using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.SalaryDeductions
{
    public class SalaryDeductionEntityTests
    {
        [Fact]
        public void SalaryDeduction_ShouldBeCreatable()
        {
            // Act
            var entity = new SalaryDeduction();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void SalaryDeduction_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new SalaryDeduction();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void SalaryDeduction_DeductionType_ShouldAcceptValue()
        {
            var entity = new SalaryDeduction { DeductionType = "Test Value" };
            entity.DeductionType.Should().Be("Test Value");
        }

        [Fact]
        public void SalaryDeduction_Currency_ShouldAcceptValue()
        {
            var entity = new SalaryDeduction { Currency = "Test Value" };
            entity.Currency.Should().Be("Test Value");
        }

    }
}
