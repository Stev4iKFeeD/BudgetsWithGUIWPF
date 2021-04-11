using System;
using Budgets.BusinessLayer.User;
using Xunit;

namespace Budgets.BusinessLayerTests
{
    public class UserTest
    {
        [Fact]
        public void ValidateValid()
        {
            // Arrange
            User user1 = new User(Guid.NewGuid(), "UsersLogin","User", "Userenko", "user@email.com");

            // Act
            bool actual = user1.Validate();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateOnlyFirstname()
        {
            // Arrange
            User user1 = new User(Guid.NewGuid(), "UsersLogin", "User", "", "");

            // Act
            bool actual = user1.Validate();
        
            // Assert
            Assert.False(actual);
        }
    }
}
