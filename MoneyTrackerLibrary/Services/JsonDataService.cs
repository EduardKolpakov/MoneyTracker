using MoneyTrackerLibrary.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyTrackerLibrary.Services
{
    public class JsonDataService : IDataService
    {
        private readonly string _filePath = "transactions.json";

        public async Task<List<Transaction>> LoadAsync()
        {
            if (!File.Exists(_filePath)) return new List<Transaction>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json);
        }

        public async Task SaveAsync(List<Transaction> transactions)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(transactions, options);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}