using System;
using Budgets.BusinessLayer;
using Budgets.BusinessLayer.Transactions;
using Xunit;

namespace Budgets.BusinessLayerTests
{
    public class TransactionTest
    {
        [Fact]
        public void ValidateValid()
        {
            // Arrange
            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", new Category(1), "", DateTimeOffset.Now);

            // Act
            bool actual = transaction1.Validate();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateOnlyCurrency()
        {
            // Arrange
            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", null, "", DateTimeOffset.Now);

            // Act
            bool actual = transaction1.Validate();

            // Assert
            Assert.False(actual);
        }
    }
}
