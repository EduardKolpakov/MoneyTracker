using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyTrackerLibrary.Model;

namespace MoneyTrackerLibrary.Services
{
    public interface IDataService
    {
        Task<List<Transaction>> LoadAsync();
        Task SaveAsync(List<Transaction> transactions);
    }
}
