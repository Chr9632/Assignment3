using assignment_3_movievault.DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace assignment_3_movievault.Services
{
    public class LogService
    {
        private readonly IMongoCollection<BsonDocument> _logs;

        public LogService(IConfiguration configuration)
        {
            var connectionString =
                configuration["MongoDB:ConnectionString"];

            var collectionName =
                configuration["MongoDB:CollectionName"];

            var mongoUrl = new MongoUrl(connectionString);
            var client = new MongoClient(mongoUrl);
            var database = client.GetDatabase(mongoUrl.DatabaseName);

            _logs = database.GetCollection<BsonDocument>(collectionName);
        }

        public async Task<List<RequestLogDto>> GetLogsAsync(
            string? username,
            DateTime? date,
            string? requestType,
            string? route)
        {
            var builder = Builders<BsonDocument>.Filter;

            var filter = builder.Exists("Properties.requestType");

            if (!string.IsNullOrWhiteSpace(username))
                filter &= builder.Eq("Properties.username", username.Trim());

            if (date.HasValue)
            {
                var start = DateTime.SpecifyKind(
                    date.Value.Date, DateTimeKind.Utc);

                var end = start.AddDays(1);

                filter &= builder.Gte("Properties.timestamp", start);
                filter &= builder.Lt("Properties.timestamp", end);
            }

            if (!string.IsNullOrWhiteSpace(requestType))
                filter &= builder.Eq(
                    "Properties.requestType",
                    requestType.Trim().ToUpperInvariant());

            if (!string.IsNullOrWhiteSpace(route))
                filter &= builder.Eq("Properties.route", route.Trim());

            var documents = await _logs.Find(filter)
                .Sort(Builders<BsonDocument>.Sort
                    .Descending("Properties.timestamp"))
                .ToListAsync();

            return documents.Select(document =>
            {
                var properties = document["Properties"].AsBsonDocument;

                return new RequestLogDto
                {
                    Username = properties["username"].AsString,
                    Timestamp = properties["timestamp"].ToUniversalTime(),
                    RequestType = properties["requestType"].AsString,
                    Route = properties["route"].AsString,
                    StatusCode = properties["statusCode"].AsInt32
                };
            }).ToList();
        }
    }
}