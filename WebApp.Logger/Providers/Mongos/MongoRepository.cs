using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;
using WebApp.Logger.Providers.Mongos.Configurations;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Providers.Mongos
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        protected readonly IMongoCollection<TDocument> _collection;
        protected readonly IMongoDatabase _database;
        protected readonly IMongoClient _client;
        private readonly LogOption _logOption;
        private readonly Mongo _mongoOptions;

        public MongoRepository(IOptions<LogOption> options)
        {
            _logOption = options.Value;
            _mongoOptions = _logOption.Provider.Mongo;

            // connection string: mongodb://[username:password@]hostname[:port][/[database][?options]]
            _client = new MongoClient(_mongoOptions.ConnectionString);

            //_client = new MongoClient(new MongoClientSettings
            //{
            //    Server = new MongoServerAddress(_mongoOptions.Server, _mongoOptions.Port),
            //    Credential = MongoCredential.CreateCredential(_mongoOptions.DatabaseName, _mongoOptions.Username, _mongoOptions.Password),
            //    SslSettings = new SslSettings
            //    {
            //        CheckCertificateRevocation = false
            //    }
            //});

            _database = _client.GetDatabase(_mongoOptions.DatabaseName);
            _collection = _database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> Filter(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public async Task<IList<TDocument>> GetPageAsync(DapperPager pager)
        {
            return await _collection.Find(x => true)
                .SortByDescending(x => x.CreatedDateUtc)
                .Skip((pager.PageIndex - 1) * pager.PageSize)
                .Limit(pager.PageSize)
                .ToListAsync();

        }

        public async Task<IList<TDocument>> GetPageAsync(Expression<Func<TDocument, bool>> filterExpression, DapperPager pager)
        {
            var d = await _collection.Find(filterExpression)
                .SortByDescending(x => x.CreatedDateUtc)
                .Skip((pager.PageIndex - 1) * pager.PageSize)
               .Limit(pager.PageSize)
               .ToListAsync();

            return d;
        }

        public virtual IEnumerable<TProjected> Filter<TProjected>(Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument Find(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual TDocument Find(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual Task<TDocument> FindAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void Insert(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public virtual Task InsertAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(IEnumerable<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public virtual async Task InsertManyAsync(IEnumerable<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void Replace(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }


        public void Delete(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            _collection.FindOneAndDelete(filter);
        }
        public void Delete(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public Task DeleteAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}
