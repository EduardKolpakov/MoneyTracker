using MoneyTrackerLibrary.Model;
using MoneyTrackerLibrary.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyTrackerLibrary.Services
{
    public class FinanceManager
    {
        private readonly IDataService _dataService;
        private List<Transaction> _transactions = new();

        public FinanceManager(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadDataAsync()
        {
            _transactions = await _dataService.LoadAsync();
        }

        public async Task SaveDataAsync()
        {
            await _dataService.SaveAsync(_transactions);
        }

        public void AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public decimal GetBalance() =>
            _transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);

        public List<Transaction> GetAllTransactions() => _transactions.ToList();
    }
}