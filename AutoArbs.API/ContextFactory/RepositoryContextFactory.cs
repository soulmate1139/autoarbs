using AutoArbs.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoArbs.API.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("AutoArbs.API"));


            //var builder = new DbContextOptionsBuilder<RepositoryContext>()
            //.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new RepositoryContext(builder.Options);
        }
    }

}
