using System.Drawing;
using Budgets.BusinessLayer;
using Xunit;

namespace Budgets.BusinessLayerTests
{
    public class CategoryTest
    {
        [Fact]
        public void ValidateValid()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            // Act
            bool actual = category1.Validate();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateOnlyName()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary" };

            // Act
            bool actual = category1.Validate();

            // Assert
            Assert.False(actual);
        }
    }
}
