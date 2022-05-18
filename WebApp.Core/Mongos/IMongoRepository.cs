using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Core.Mongos.Configurations;

namespace WebApp.Core.Mongos
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> Filter(Expression<Func<TDocument, bool>> filterExpression);
        IEnumerable<TProjected> Filter<TProjected>(Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);
        
        TDocument Find(string id);
        TDocument Find(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindAsync(string id);
        
        void Insert(TDocument document);
        Task InsertAsync(TDocument document);
        void InsertMany(ICollection<TDocument> documents);
        Task InsertManyAsync(ICollection<TDocument> documents);

        void Replace(TDocument document);
        Task ReplaceAsync(TDocument document);

        void Delete(string id);
        void Delete(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteAsync(string id);
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
