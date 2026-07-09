using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DepartmentApp.EntityFrameworkCore
{
    public class DepartmentAppDbContextFactory : IDesignTimeDbContextFactory<DepartmentAppDbContext>
    {
        public DepartmentAppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DepartmentApp.Web.Host"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connStr = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(connStr);
            return new DepartmentAppDbContext(builder.Options);
        }
    }
}
