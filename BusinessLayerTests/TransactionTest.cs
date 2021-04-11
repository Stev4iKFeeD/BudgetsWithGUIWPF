using Budgets.BusinessLayer;
using Xunit;

namespace Budgets.BusinessLayerTests
{
    public class TransactionTest
    {
        [Fact]
        public void ValidateValid()
        {
            // Arrange
            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = new Category(1) };

            // Act
            bool actual = transaction1.Validate();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateOnlyCurrency()
        {
            // Arrange
            Transaction transaction1 = new Transaction(1) { Currency = "UAH" };

            // Act
            bool actual = transaction1.Validate();

            // Assert
            Assert.False(actual);
        }
    }
}
