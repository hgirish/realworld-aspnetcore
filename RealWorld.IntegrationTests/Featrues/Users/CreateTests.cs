using Microsoft.EntityFrameworkCore;
using RealWorld.Features.Users;
using RealWorld.Infrastructure.Security;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RealWorld.IntegrationTests.Featrues.Users
{
    public    class CreateTests  : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_UserAsync()
        {
            var command = new Create.Command
            {
                User = new Create.UserData
                {
                    Email = "email@example.com",
                    Password = "password",
                    Username = "username"
                }
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Persons.Where(
                d => d.Email == command.User.Email).SingleOrDefaultAsync());
            Assert.NotNull(created);
            Assert.Equal(created.Hash, new PasswordHasher().Hash("password", created.Salt));
        }
        [Fact]
        public async Task Invalid_email_throws_Exception()
        {
            const string email = "invalidemail";
            var command = new Create.Command
            {
                User = new Create.UserData
                {
                    Email = email,
                    Password = "password",
                    Username = "username"
                }
            };

            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                async () => await SendAsync(command));
            Assert.Contains("'Email' is not a valid email", ex.Message);


        }
        [Fact]
        public async Task empty_password_throws_Exception()
        {
            const string email = "email@example.com";

            const string password = "";
            var command = new Create.Command
            {
                User = new Create.UserData
                {
                    Email = email,
                    Password = password,
                    Username = "username"
                }
            };

            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                async () => await SendAsync(command));
            Assert.Contains("'Password' should not be empty", ex.Message);
        }
        [Fact]
        public async Task empty_username_throws_Exception()
        {
            const string email = "email@example.com";

            const string password = "password";
            const string username = "";
            var command = new Create.Command
            {
                User = new Create.UserData
                {
                    Email = email,
                    Password = password,
                    Username = username
                }
            };

            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                async () => await SendAsync(command));
            Assert.Contains("'Username' should not be empty", ex.Message);
        }
        [Fact]
        public async Task empty_email_throws_Exception()
        {
            const string email = "";

            const string password = "password";
            const string username = "username";
            var command = new Create.Command
            {
                User = new Create.UserData
                {
                    Email = email,
                    Password = password,
                    Username = username
                }
            };

            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                async () => await SendAsync(command));
            Assert.Contains("'Email' should not be empty", ex.Message);
        }
    }
}
