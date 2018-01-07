using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcingCQRS.ReadModel.Tests
{

    [Trait("Type", "Integration")]
    public class MongoDBTest : MongoDBTestBase
    {
        private const string CollectionName = "TestEntity";

        [Fact]
        public async Task CanInsertAnItemInCollection()
        {
            string id = await InsertTestElementWithQuantity(5);
            var test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();

            Assert.NotNull(test);
            Assert.Equal(id, test.Id);
            Assert.Equal(5, test.Quantity);
        }

        [Fact]
        public async Task CanIncrementQuantity()
        {
            string id = await InsertTestElementWithQuantity(5);

            await mongoDB.GetCollection<TestEntity>(CollectionName)
                .FindOneAndUpdateAsync(x => x.Id == id, Builders<TestEntity>.Update.Inc(x => x.Quantity, 3));

            var test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();

            Assert.Equal(8, test.Quantity);
        }

        [Fact]
        public async Task CanUpdateQuantity()
        {
            string id = await InsertTestElementWithQuantity(5);

            var test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();

            await mongoDB.GetCollection<TestEntity>(CollectionName)
                .UpdateOneAsync(x => x.Id == id, Builders<TestEntity>.Update.Set(x => x.Quantity, 8));

            test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();

            Assert.Equal(8, test.Quantity);
        }

        [Fact]
        public async Task IdIsNotAutomaticallyAssigned()
        {
            await mongoDB.GetCollection<TestEntity>(CollectionName)
                .InsertOneAsync(new TestEntity(null)
                {
                    Quantity = 7
                });

            var test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(FilterDefinition<TestEntity>.Empty)
                .SingleAsync();

            Assert.NotNull(test);
            Assert.Null(test.Id);
        }

        [Fact]
        public async Task ListExistingDBs()
        {
            await InsertTestElementWithQuantity(5);

            var databases = (await client.ListDatabasesAsync()).ToList();
            var mongoDBTestDatabase = databases.SingleOrDefault(x => x["name"].AsString == GetType().Name);

            Assert.NotNull(mongoDBTestDatabase);
        }

        private async Task<string> InsertTestElementWithQuantity(int quantity)
        {
            string id = Guid.NewGuid().ToString();

            await mongoDB.GetCollection<TestEntity>(CollectionName)
                .InsertOneAsync(new TestEntity(id)
                {
                    Quantity = quantity
                });
            return id;
        }
    }
}
