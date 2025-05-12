using MoneyTrackerLibrary.Model;
using MoneyTrackerLibrary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrackerApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FinanceManager _financeManager;
        private readonly JsonDataService _dataService = new();

        public ObservableCollection<Transaction> Transactions { get; set; } = new();

        private string _description;
        private decimal _amount;
        private TransactionType _type = TransactionType.Expense;

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public TransactionType Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public decimal Balance => _financeManager.GetBalance();

        public MainViewModel()
        {
            _financeManager = new FinanceManager(_dataService);
            LoadData();
        }

        private async void LoadData()
        {
            await _financeManager.LoadDataAsync();
            foreach (var t in _financeManager.GetAllTransactions())
            {
                Transactions.Add(t);
            }
            OnPropertyChanged(nameof(Balance));
        }

        public async void AddTransaction()
        {
            var transaction = new Transaction
            {
                Id = Transactions.Count + 1,
                Date = DateTime.Now,
                Description = Description,
                Amount = Amount,
                Type = Type
            };

            _financeManager.AddTransaction(transaction);
            Transactions.Add(transaction);

            await _financeManager.SaveDataAsync();

            OnPropertyChanged(nameof(Balance));

            // Сброс полей
            Description = string.Empty;
            Amount = 0;
            Type = TransactionType.Expense;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
