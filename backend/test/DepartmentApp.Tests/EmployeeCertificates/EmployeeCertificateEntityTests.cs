using System;
using Xunit;
using FluentAssertions;
using DepartmentApp.Entities;

namespace DepartmentApp.Tests.EmployeeCertificates
{
    public class EmployeeCertificateEntityTests
    {
        [Fact]
        public void EmployeeCertificate_ShouldBeCreatable()
        {
            // Act
            var entity = new EmployeeCertificate();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void EmployeeCertificate_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new EmployeeCertificate();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void EmployeeCertificate_CertificateName_ShouldAcceptValue()
        {
            var entity = new EmployeeCertificate { CertificateName = "Test Value" };
            entity.CertificateName.Should().Be("Test Value");
        }

    }
}
