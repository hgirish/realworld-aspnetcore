using RealWorld.Domain;
using RealWorld.Features.Users;
using RealWorld.Infrastructure.Errors;
using RealWorld.Infrastructure.Security;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RealWorld.IntegrationTests.Featrues.Users
{
    public class LoginTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Login()
        {
            var salt = Guid.NewGuid().ToByteArray();
            const string password = "password";
            const string email = "email@example.com";
            const string username = "username";
            var person = new Person
            {
                Username = username,
                Email = email,
                Hash = new PasswordHasher().Hash(password, salt),
                Salt = salt
            };

            await InsertAsync(person);

            var command = new Login.Command
            {
                User = new Login.UserData
                {
                    Email = email,
                    Password = password
                }
            };

            var user = await SendAsync(command);

            Assert.NotNull(user?.User);
            Assert.Equal(user.User.Email, command.User.Email);
            Assert.Equal(username, user.User.Username);
            Assert.NotNull(user.User.Token);
            Console.WriteLine(user.User.Token);


        }

        [Theory]
        [InlineData("email@example.com","password","email@example.com","wrongpassword")]
        [InlineData("email@example.com", "password", "wrongemail@example.com", "password")]
        public async Task  Expect_RestException_On_Invalid_Credentials(
            string email,string password, string loginEmail, string loginPassword)
        {
            var salt = Guid.NewGuid().ToByteArray();
           
            const string username = "username";
            var person = new Person
            {
                Username = username,
                Email = email,
                Hash = new PasswordHasher().Hash(password, salt),
                Salt = salt
            };

            await InsertAsync(person);

            var command = new Login.Command
            {
                User = new Login.UserData
                {
                    Email = loginEmail,
                    Password = loginPassword
                }
            };

            RestException ex = await Assert.ThrowsAsync<RestException>(async () => await SendAsync(command));
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, ex.Code);
        }

        
    }
}
