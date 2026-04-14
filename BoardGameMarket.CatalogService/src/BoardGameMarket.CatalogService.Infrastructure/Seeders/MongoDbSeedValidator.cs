using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Infrastructure.Seeders
{
    internal class MongoDbSeedValidator : ISeedValidator
    {
        private readonly IMongoDatabase _database;

        public MongoDbSeedValidator(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> IsSeeded()
        {
            IMongoCollection<BsonDocument> markerCollection = _database.GetCollection<BsonDocument>("_seed_marker");

            return await markerCollection.Find(Builders<BsonDocument>.Filter.Eq("_id", 1)).AnyAsync();

        }
    }
}
