using MoneyTrackerLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyTrackerLibrary.Services
{
    public interface IDataService
    {
        Task<List<Transaction>> LoadAsync();
        Task SaveAsync(List<Transaction> transactions);
    }
}