using Abstractions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace MongoDb
{
    public class Repository : IRepository
    {

        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private static ILogger _logger;

        public Repository(ILogger<Repository> logger, IRepositoryConfiguration configuration)
        {
            _logger = logger;

            if (string.IsNullOrWhiteSpace(configuration.Path))
                return;

            var connection = new MongoUrlBuilder(configuration.Path);

            var settings = MongoClientSettings.FromUrl(connection.ToMongoUrl());

            if (configuration.UseSSL)
            {
                settings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };
            }

            _client = new MongoClient(settings);
            _database = _client.GetDatabase(connection.DatabaseName);

        }

        public async Task Add<T>(T item) where T : class, new()
        {
            await _database.GetCollection<T>(typeof(T).Name)
                .InsertOneAsync(item);
        }

        public async Task AddMany<T>(IEnumerable<T> items) where T : class, new()
        {
            await _database.GetCollection<T>(typeof(T).Name)
                .InsertManyAsync(items);
        }

        public async Task<IQueryable<T>> All<T>() where T : class, new()
        {
            return await Task.FromResult(_database.GetCollection<T>(typeof(T).Name).AsQueryable());
        }

        public async Task<IQueryable<T>> Where<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return (await All<T>()).Where(expression).AsQueryable();
        }

        public bool CollectionEmpty<T>() where T : class, new()
        {
            try
            {
                var collection = _database.GetCollection<T>(typeof(T).Name);
                var filter = new BsonDocument();
                var totalCount = collection.CountDocuments(filter);
                return totalCount == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public async Task Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
           await _database.GetCollection<T>(typeof(T).Name)
                .DeleteManyAsync(expression);
        }

        public async Task<T> Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            try
            {
                return (await Where(expression)).SingleOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return default;
            }
        }

        public async Task Update<T>(Expression<Func<T, bool>> expression, T item) where T : class, new()
        {
            await _database.GetCollection<T>(typeof(T).Name)
                .ReplaceOneAsync(expression, item);
        }

    }
}
