using speedyairly.Constants;
using speedyairly.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace speedyairly.Services
{
    internal class JsonOrderProvider : IOrderProvider
    {
        private readonly string _fileName;

        public JsonOrderProvider(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
            }

            _fileName = fileName;
        }

        public IEnumerable<IOrder> GetOrders()
        {
            var json = File.ReadAllText(_fileName);
            var orders = JsonSerializer.Deserialize<Dictionary<string, JsonOrder>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
            return (orders ?? []).Select(o => new Order(o.Key, o.Value.Destination));
        }

        private sealed class JsonOrder
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Airport Destination { get; set; }
        }
    }
}
