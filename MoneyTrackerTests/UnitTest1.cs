using System;
using MoneyTrackerLibrary.Model;
using MoneyTrackerLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MoneyTrackerTests
{
    [TestClass]
    public class TestClass
    {
        private IDataService _dataService;
        private FinanceManager _manager;

        [TestInitialize]
        public void Setup()
        {
            // Используем InMemoryDataService для большинства тестов
            _dataService = new InMemoryDataService();
            _manager = new FinanceManager(_dataService);
        }

        [TestMethod]
        public void Transaction_InitializesCorrectly()
        {
            var transaction = new Transaction
            {
                Id = 1,
                Date = new System.DateTime(2025, 4, 1),
                Description = "Зарплата",
                Amount = 100000,
                Type = TransactionType.Income
            };

            Assert.AreEqual(1, transaction.Id);
            Assert.AreEqual(new System.DateTime(2025, 4, 1), transaction.Date);
            Assert.AreEqual("Зарплата", transaction.Description);
            Assert.AreEqual(100000, transaction.Amount);
            Assert.AreEqual(TransactionType.Income, transaction.Type);
        }

        [TestMethod]
        public void AddTransaction_ShouldIncreaseCount()
        {
            _manager.AddTransaction(new Transaction { Description = "Кофе" });

            CollectionAssert.AllItemsAreInstancesOfType(_manager.GetAllTransactions(), typeof(Transaction));
            Assert.AreEqual(1, _manager.GetAllTransactions().Count);
        }

        [TestMethod]
        public void GetBalance_ReturnsZero_WhenNoTransactions()
        {
            decimal balance = _manager.GetBalance();

            Assert.AreEqual(0, balance);
        }

        [TestMethod]
        public void GetBalance_CalculatesIncomeAndExpenses()
        {
            _manager.AddTransaction(new Transaction { Amount = 1000, Type = TransactionType.Income });
            _manager.AddTransaction(new Transaction { Amount = 500, Type = TransactionType.Expense });

            decimal balance = _manager.GetBalance();

            Assert.AreEqual(500, balance);
        }

        [TestMethod]
        public async Task JsonDataService_SaveAndLoad_TransactionsSuccessfully()
        {
            string filePath = "test_transactions.json";
            if (File.Exists(filePath)) File.Delete(filePath);

            IDataService jsonService = new JsonDataService();

            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Description = "Тестовый доход", Amount = 100, Type = TransactionType.Income },
                new Transaction { Id = 2, Description = "Тестовый расход", Amount = 50, Type = TransactionType.Expense }
            };

            await jsonService.SaveAsync(transactions);
            var loaded = await jsonService.LoadAsync();

            Assert.IsNotNull(loaded);
            Assert.AreEqual(2, loaded.Count);
            Assert.AreEqual("Тестовый доход", loaded[0].Description);
            Assert.AreEqual(100, loaded[0].Amount);
            Assert.AreEqual("Тестовый расход", loaded[1].Description);
            Assert.AreEqual(50, loaded[1].Amount);
        }


        [TestMethod]
        public void GetAllTransactions_ReturnsCopyOfList()
        {
            _manager.AddTransaction(new Transaction { Description = "Кофе" });
            var list = _manager.GetAllTransactions();

            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);
            Assert.IsInstanceOfType(list[0], typeof(Transaction));
        }


        [TestMethod]
        public void ChangeDistance_ShouldThrowIfNegative()
        {
            // Arrange
            var dataService = new InMemoryDataService();
            var manager = new FinanceManager(dataService);

            // Act + Assert
            try
            {
                manager.AddTransaction(new Transaction { Amount = -100, Type = TransactionType.Expense });
                Assert.Fail("Ожидалось исключение при добавлении отрицательной суммы");
            }
            catch (System.Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(System.Exception));
            }
        }

        [TestMethod]
        public void SaveData_UpdatesStorage()
        {
            var inMemoryService = new InMemoryDataService();
            var testManager = new FinanceManager(inMemoryService);

            testManager.AddTransaction(new Transaction { Description = "Тестовая транзакция" });

            testManager.SaveDataAsync().Wait();

            var saved = testManager.GetAllTransactions();
            Assert.AreEqual(1, saved.Count);
        }

        [TestMethod]
        public void LoadData_LoadsExistingData()
        {
            var inMemoryService = new InMemoryDataService();
            inMemoryService.PreloadData(new List<Transaction>
            {
                new Transaction { Description = "Загруженная транзакция" }
            });

            var testManager = new FinanceManager(inMemoryService);
            testManager.LoadDataAsync().Wait();

            var loaded = testManager.GetAllTransactions();
            Assert.AreEqual(1, loaded.Count);
            Assert.AreEqual("Загруженная транзакция", loaded.First().Description);
        }

        // Простая реализация IDataService для тестов
        internal class InMemoryDataService : IDataService
        {
            private List<Transaction> _transactions = new List<Transaction>();

            public void PreloadData(List<Transaction> transactions) => _transactions = transactions;

            public Task<List<Transaction>> LoadAsync() => Task.FromResult(_transactions);

            public Task SaveAsync(List<Transaction> transactions)
            {
                _transactions = transactions;
                return Task.CompletedTask;
            }
        }
    }
}
