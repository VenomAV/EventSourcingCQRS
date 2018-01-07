using EventSourcingCQRS.ReadModel.Common;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSourcingCQRS.ReadModel.Persistence
{
    public class MongoDBRepository<T> : IRepository<T>
        where T : IReadEntity
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoDBRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }

        private string CollectionName => typeof(T).Name;

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            var cursor = await mongoDatabase.GetCollection<T>(CollectionName)
                .FindAsync(predicate);
            return cursor.ToEnumerable();
        }

        public Task<T> GetByIdAsync(string id)
        {
            return mongoDatabase.GetCollection<T>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();
        }

        public async Task InsertAsync(T entity)
        {
            try
            {
                await mongoDatabase.GetCollection<T>(CollectionName)
                    .InsertOneAsync(entity);
            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException($"Error inserting entity {entity.Id}", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                var result = await mongoDatabase.GetCollection<T>(CollectionName)
                    .ReplaceOneAsync(x => x.Id == entity.Id, entity);

                if (result.MatchedCount != 1)
                {
                    throw new RepositoryException($"Missing entoty {entity.Id}");
                }
            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException($"Error updating entity {entity.Id}", ex);
            }
        }
    }
}
