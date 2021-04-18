using System;
using System.Collections.Generic;
using System.Drawing;
using Budgets.BusinessLayer;
using Budgets.BusinessLayer.Transactions;
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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", initialBalance, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", category1, "", DateTimeOffset.Now);
            Transaction transaction2 = new Transaction(Guid.NewGuid(), -5.19M, "UAH", category2, "", DateTimeOffset.Now);
            Transaction transaction3 = new Transaction(Guid.NewGuid(), 32.32M, "UAH", category1, "", DateTimeOffset.Now);

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
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset today = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);

            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };
            Category category2 = new Category(2) { Name = "Health", Color = Color.Green };

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", category1, "", now);
            Transaction transaction2 = new Transaction(Guid.NewGuid(), -5.19M, "UAH", category2, "", now);
            Transaction transaction3 = new Transaction(Guid.NewGuid(), 32.32M, "UAH", category1, "", now);

            wallet1.AddTransaction(transaction1);
            wallet1.AddTransaction(transaction2);
            wallet1.AddTransaction(transaction3);

            decimal expected = transaction1.Sum + transaction3.Sum;

            // Act
            decimal actual = wallet1.Incomes(today, now);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IncomesForCurrentMonthTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", category1, "", DateTimeOffset.Now);
            Transaction transaction2 = new Transaction(Guid.NewGuid(), 132.32M, "UAH", category1, "", DateTimeOffset.Now);
            Transaction transaction3 = new Transaction(Guid.NewGuid(), 43.76M, "UAH", category1, "",
                new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month - 1, DateTimeOffset.Now.Day,
                    DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second,
                    DateTimeOffset.Now.Offset));

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
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset today = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);

            Category category1 = new Category(1) { Name = "Salary", Color = Color.Blue };
            Category category2 = new Category(2) { Name = "Health", Color = Color.Green };

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", category1, "", now);
            Transaction transaction2 = new Transaction(Guid.NewGuid(), -5.19M, "UAH", category2, "", now);
            Transaction transaction3 = new Transaction(Guid.NewGuid(), -32.32M, "UAH", category2, "", now);

            wallet1.AddTransaction(transaction1);
            wallet1.AddTransaction(transaction2);
            wallet1.AddTransaction(transaction3);

            decimal expected = Math.Abs(transaction2.Sum + transaction3.Sum);

            // Act
            decimal actual = wallet1.Expenses(today, now);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExpensesForCurrentMonthTest()
        {
            // Arrange
            Category category1 = new Category(1) { Name = "Health", Color = Color.Green };

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), -10.10M, "UAH", category1, "", DateTimeOffset.Now);
            Transaction transaction2 = new Transaction(Guid.NewGuid(), -132.32M, "UAH", category1, "", DateTimeOffset.Now);
            Transaction transaction3 = new Transaction(Guid.NewGuid(), -43.76M, "UAH", category1, "",
                new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month - 1, DateTimeOffset.Now.Day,
                    DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second,
                    DateTimeOffset.Now.Offset));
            
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

            Wallet walletNoCategories = new Wallet(Guid.NewGuid(), "Without categories", "desc", "UAH", 0, Guid.NewGuid());

            Wallet walletHealthCategory = new Wallet(Guid.NewGuid(), "With category (Health)", "desc", "USD", 0, Guid.NewGuid());
            walletHealthCategory.AddCategory(categoryHealth);

            Transaction transactionEat = new Transaction(Guid.NewGuid(), -10.10M, "UAH", categoryEat, "", DateTimeOffset.Now);
            Transaction transactionHealth = new Transaction(Guid.NewGuid(), -2.02M, "UAH", categoryHealth, "", DateTimeOffset.Now);

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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", category1, "", DateTimeOffset.Now);
            Transaction transaction2 = new Transaction(Guid.NewGuid(), 5.19M, "UAH", category1, "", DateTimeOffset.Now);

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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", category1, "", DateTimeOffset.Now);

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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            Transaction transaction1 = new Transaction(Guid.NewGuid(), 10.10M, "UAH", category1, "", DateTimeOffset.Now);

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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            List<Transaction> transactions = new List<Transaction>();
            for (int i = 1; i <= 12; i++)
            {
                Transaction transaction1 = new Transaction(Guid.NewGuid(), i * 10.10M, "UAH", category1, "", DateTimeOffset.Now);
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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            List<Transaction> transactions = new List<Transaction>();
            for (int i = 1; i <= 12; i++)
            {
                Transaction transaction1 = new Transaction(Guid.NewGuid(), i * 10.10M, "UAH", category1, "", DateTimeOffset.Now);
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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

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

            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

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
        
            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

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
        
            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

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
            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

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
            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "UAH", 0, Guid.NewGuid());

            // Act
            bool actual = wallet1.Validate();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateOnlyName()
        {
            // Arrange
            Wallet wallet1 = new Wallet(Guid.NewGuid(), "Wall_let", "desc", "", 0, Guid.NewGuid());

            // Act
            bool actual = wallet1.Validate();

            // Assert
            Assert.False(actual);
        }
    }
}
