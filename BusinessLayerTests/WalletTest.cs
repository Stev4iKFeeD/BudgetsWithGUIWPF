using System;
using System.Collections.Generic;
using System.Drawing;
using Budgets.BusinessLayer;
using Budgets.BusinessLayer.Wallets;
using Xunit;

namespace Budgets.BusinessLayerTests
{
    public class WalletTest
    {
        [Fact]
        public void CurrentBalanceTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };
            Category category2 = new Category(2) { Name = "Health", Color = Color.Green };

            decimal initialBalance = 50.00M;

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH", InitialBalance = initialBalance };

            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction2 = new Transaction(2) { Sum = -5.19M, Currency = "UAH", Category = category2, Date = DateTime.Today };
            Transaction transaction3 = new Transaction(3) { Sum = 32.32M, Currency = "UAH", Category = category1, Date = DateTime.Today };

            wallet1.AddTransaction(transaction1);
            wallet1.AddTransaction(transaction2);
            wallet1.AddTransaction(transaction3);

            decimal expected = initialBalance + transaction1.Sum + transaction2.Sum + transaction3.Sum;

            // Act
            decimal actual = wallet1.CurrentBalance;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IncomesTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };
            Category category2 = new Category(2) { Name = "Health", Color = Color.Green };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction2 = new Transaction(2) { Sum = -5.19M, Currency = "UAH", Category = category2, Date = DateTime.Today };
            Transaction transaction3 = new Transaction(3) { Sum = 32.32M, Currency = "UAH", Category = category1, Date = DateTime.Today };

            wallet1.AddTransaction(transaction1);
            wallet1.AddTransaction(transaction2);
            wallet1.AddTransaction(transaction3);

            decimal expected = transaction1.Sum + transaction3.Sum;

            // Act
            decimal actual = wallet1.Incomes(DateTime.Today, DateTime.Today);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IncomesForCurrentMonthTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction2 = new Transaction(2) { Sum = 132.32M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction3 = new Transaction(3)
            {
                Sum = 43.76M,
                Currency = "UAH",
                Category = category1,
                Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day)
            };

            wallet1.AddTransaction(transaction1);
            wallet1.AddTransaction(transaction2);
            wallet1.AddTransaction(transaction3);

            decimal expected = transaction1.Sum + transaction2.Sum;

            // Act
            decimal actual = wallet1.IncomesForCurrentMonth();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExpensesTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };
            Category category2 = new Category(2) { Name = "Health", Color = Color.Green };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction2 = new Transaction(2) { Sum = -5.19M, Currency = "UAH", Category = category2, Date = DateTime.Today };
            Transaction transaction3 = new Transaction(3) { Sum = -32.32M, Currency = "UAH", Category = category2, Date = DateTime.Today };

            wallet1.AddTransaction(transaction1);
            wallet1.AddTransaction(transaction2);
            wallet1.AddTransaction(transaction3);

            decimal expected = Math.Abs(transaction2.Sum + transaction3.Sum);

            // Act
            decimal actual = wallet1.Expenses(DateTime.Today, DateTime.Today);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExpensesForCurrentMonthTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Health", Color = Color.Green };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            Transaction transaction1 = new Transaction(1) { Sum = -10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction2 = new Transaction(2) { Sum = -132.32M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction3 = new Transaction(3)
            {
                Sum = -43.76M,
                Currency = "UAH",
                Category = category1,
                Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day)
            };

            wallet1.AddTransaction(transaction1);
            wallet1.AddTransaction(transaction2);
            wallet1.AddTransaction(transaction3);

            decimal expected = Math.Abs(transaction1.Sum + transaction2.Sum);

            // Act
            decimal actual = wallet1.ExpensesForCurrentMonth();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddTransactionTest()
        {
            // Arrange
            Category categoryHealth = new Category(1) { Name = "Health", Color = Color.Green };
            Category categoryEat = new Category(2) { Name = "Eat", Color = Color.Red };

            Wallet walletNoCategories = new Wallet() { Name = "Without categories", Currency = "UAH" };

            Wallet walletHealthCategory = new Wallet() { Name = "With category (Health)", Currency = "USD" };
            walletHealthCategory.AddCategory(categoryHealth);

            Transaction transactionEat = new Transaction(1) { Sum = -10.10M, Currency = "UAH", Category = categoryEat, Date = DateTime.Today};
            Transaction transactionHealth = new Transaction(2) { Sum = -2.02M, Currency = "UAH", Category = categoryHealth, Date = DateTime.Today};

            // Act
            bool wncAddTe = walletNoCategories.AddTransaction(transactionEat);
            bool wncAddTh = walletNoCategories.AddTransaction(transactionHealth);
            bool whcAddTe = walletHealthCategory.AddTransaction(transactionEat);
            bool whcAddTh = walletHealthCategory.AddTransaction(transactionHealth);

            // Assert
            Assert.True(wncAddTe);
            Assert.True(wncAddTh);
            Assert.Equal(2, walletNoCategories.GetTenRecentlyAddedTransactions().Count);
            Assert.False(whcAddTe);
            Assert.True(whcAddTh);
            Assert.Single(walletHealthCategory.GetTenRecentlyAddedTransactions());
        }

        [Fact]
        public void RemoveTransactionByValueTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
            Transaction transaction2 = new Transaction(2) { Sum = 5.19M, Currency = "UAH", Category = category1, Date = DateTime.Today };

            wallet1.AddTransaction(transaction1);

            // Act
            bool remT1 = wallet1.RemoveTransaction(transaction1);
            bool remT2 = wallet1.RemoveTransaction(transaction2);

            // Assert
            Assert.True(remT1);
            Assert.False(remT2);
        }

        [Fact]
        public void RemoveTransactionByIndexTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };

            wallet1.AddTransaction(transaction1);

            // Act
            bool remT1 = wallet1.RemoveTransaction(1);
            bool remT2 = wallet1.RemoveTransaction(0);

            // Assert
            Assert.True(remT1);
            Assert.False(remT2);
        }

        [Fact]
        public void GetTransactionTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            Transaction transaction1 = new Transaction(1) { Sum = 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };

            wallet1.AddTransaction(transaction1);

            // Act
            Transaction actualNonNull = wallet1.GetTransaction(1);
            Transaction actualNull = wallet1.GetTransaction(0);

            // Assert
            Assert.Equal(transaction1, actualNonNull);
            Assert.Null(actualNull);
        }

        [Fact]
        public void GetTransactionsTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            List<Transaction> transactions = new List<Transaction>();
            for (int i = 1; i <= 12; i++)
            {
                Transaction transaction1 = new Transaction(i) { Sum = i * 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
                transactions.Add(transaction1);
                wallet1.AddTransaction(transaction1);
            }

            // Act
            List<Transaction> actual = wallet1.GetTransactions(1, 10);
            List<Transaction> actualNull = wallet1.GetTransactions(0, 100);

            // Assert
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(transactions[i], actual[i]);
            }
            Assert.Null(actualNull);
        }

        [Fact]
        public void GetTenRecentlyAddedTransactionsTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            List<Transaction> transactions = new List<Transaction>();
            for (int i = 1; i <= 12; i++)
            {
                Transaction transaction1 = new Transaction(i) { Sum = i * 10.10M, Currency = "UAH", Category = category1, Date = DateTime.Today };
                transactions.Add(transaction1);
                wallet1.AddTransaction(transaction1);
            }

            // Act
            List<Transaction> actual = wallet1.GetTenRecentlyAddedTransactions();

            // Assert
            for (int i = actual.Count - 1; i >= 0; i--)
            {
                Assert.Equal(transactions[i + (transactions.Count - actual.Count)], actual[i]);
            }
        }

        [Fact]
        public void AddCategoryTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Health", Color = Color.Green };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            // Act
            bool actualAddFirstTime = wallet1.AddCategory(category1);
            bool actualAddSecondTime = wallet1.AddCategory(category1);

            // Assert
            Assert.True(actualAddFirstTime);
            Assert.False(actualAddSecondTime);
        }

        [Fact]
        public void RemoveCategoryByValueTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Health", Color = Color.Green };
            Category category2 = new Category(2) { Name = "Eat", Color = Color.Red };

            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };
        
            wallet1.AddCategory(category1);
        
            // Act
            bool remC1 = wallet1.RemoveCategory(category1);
            bool remC2 = wallet1.RemoveCategory(category2);
        
            // Assert
            Assert.True(remC1);
            Assert.False(remC2);
        }
        
        [Fact]
        public void RemoveCategoryByIndexTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };
        
            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };
        
            wallet1.AddCategory(category1);
        
            // Act
            bool remC1 = wallet1.RemoveCategory(1);
            bool remC2 = wallet1.RemoveCategory(0);
        
            // Assert
            Assert.True(remC1);
            Assert.False(remC2);
        }
        
        [Fact]
        public void GetCategoryTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };
        
            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };
        
            wallet1.AddCategory(category1);
        
            // Act
            Category actualNonNull = wallet1.GetCategory(1);
            Category actualNull = wallet1.GetCategory(0);
        
            // Assert
            Assert.Equal(category1, actualNonNull);
            Assert.Null(actualNull);
        }
        
        [Fact]
        public void GetCategoriesTest()
        {
            // Arrange
            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            List<Category> categories = new List<Category>();
            for (int i = 0; i < 10; i++)
            {
                Category category1 = new Category(i) { Name = "Salary" + i, Color = Color.Blue };
                categories.Add(category1);
                wallet1.AddCategory(category1);
            }

            // Act
            List<Category> actual = wallet1.GetCategories();
        
            // Assert
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(categories[i], actual[i]);
            }
        }

        [Fact]
        public void ValidateValid()
        {
            // Arrange
            Wallet wallet1 = new Wallet() { Name = "Wall_let", Currency = "UAH" };

            // Act
            bool actual = wallet1.Validate();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateOnlyName()
        {
            // Arrange
            Wallet wallet1 = new Wallet() { Name = "Wall_let" };

            // Act
            bool actual = wallet1.Validate();

            // Assert
            Assert.False(actual);
        }
    }
}
