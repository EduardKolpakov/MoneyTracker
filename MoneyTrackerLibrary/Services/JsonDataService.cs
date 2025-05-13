using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MoneyTrackerLibrary.Model;
using System.Text.Json.Serialization;
using System.Xml;

namespace MoneyTrackerLibrary.Services
{
    public class JsonDataService : IDataService
    {
        private readonly string _filePath = "transactions.json";

        public async Task<List<Transaction>> LoadAsync()
        {
            if (!File.Exists(_filePath)) return new List<Transaction>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Transaction>>(json);
        }

        public async Task SaveAsync(List<Transaction> transactions)
        {
            var json = JsonConvert.SerializeObject(transactions, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}