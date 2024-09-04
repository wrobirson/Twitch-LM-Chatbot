using LiteDB;
using System.Linq.Expressions;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Persistence.Reopsotories
{
    public abstract class LiteDbRepositoryBase<T> : IRepository<T> where T : Entity
    {
        private readonly ILiteDatabase _db;
        private readonly string _collectionName;

        public LiteDbRepositoryBase(ILiteDatabase db, string collectionName)
        {
            _db = db;
            _collectionName = collectionName;
        }

        protected ILiteCollection<T> GetCollection()
        {
            return _db.GetCollection<T>(_collectionName);
        }

        public virtual List<T> FindAll()
        {
            return GetCollection().FindAll().ToList();
        }

        public virtual void Insert(T entity)
        {
            GetCollection().Insert(entity);
        }

        public virtual void Delete(T entity)
        {
            GetCollection().Delete(entity.Id);
        }

        public virtual void Update(T entity)
        {
            GetCollection().Update(entity);
        }

        public T FindById(int id)
        {
            return GetCollection().FindById(id);
        }

        public int Count()
        {
            return GetCollection().Count();
        }

        public List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return GetCollection().Query()
                .Where(predicate).ToList();
        }
    }
}
