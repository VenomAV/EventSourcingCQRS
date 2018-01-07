using MongoDB.Driver;
using System;

namespace EventSourcingCQRS.ReadModel.Tests
{
    public class MongoDBTestBase : IDisposable
    {
        protected IMongoDatabase mongoDB;
        protected MongoClient client;

        public MongoDBTestBase(string database = null)
        {
            database = database ?? GetType().Name;
            client = new MongoClient("mongodb://localhost:27017");
            mongoDB = client.GetDatabase(database);
        }

        public void Dispose()
        {
            client.DropDatabase(GetType().Name);
        }
    }
}
