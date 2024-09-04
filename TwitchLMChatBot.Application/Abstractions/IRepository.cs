
using System.Linq.Expressions;

namespace TwitchLMChatBot.Application.Abstractions
{
    public interface IRepository<T>
    {
        void Insert(T entity);
        T FindById(int id);
        List<T> FindAll();
        void Delete(T entity);
        void Update(T entity);
        int Count();

        List<T> Find(Expression<Func<T, bool>> predicate);
    }
}