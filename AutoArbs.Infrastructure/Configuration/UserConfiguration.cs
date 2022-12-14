using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using AutoArbs.Domain.Models;

namespace AutoArbs.API.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData
            (
            new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@gmail.com",
                Password = "John123",
                Balance= 0,
                Bonus= 0,
                IsActive=false,
            },
            new User
            {
                FirstName = "Mary",
                LastName = "Jane",
                Email = "mary@gmail.com",
                Password = "mary123",
                Balance= 50,
                Bonus= 0,
                IsActive=false,
            }
            );
        }
    }

}
