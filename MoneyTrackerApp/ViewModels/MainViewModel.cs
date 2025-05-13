using MoneyTrackerLibrary.Model;
using MoneyTrackerLibrary.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyTrackerApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FinanceManager _financeManager;
        private readonly IDataService _dataService = new JsonDataService();

        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

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
            if (Amount <= 0)
            {
                MessageBox.Show("Сумма должна быть больше нуля.");
                return;
            }

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