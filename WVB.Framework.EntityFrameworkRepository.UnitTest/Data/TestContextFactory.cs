using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Data
{
    public class TestContextFactory : IDesignTimeDbContextFactory<TestContext>
    {
        public TestContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false)
                .Build();
            
            return new TestContext(Enum.Database.SqlServer, configuration.GetConnectionString("RepositoryConnectionString"));
        }
    }
}
