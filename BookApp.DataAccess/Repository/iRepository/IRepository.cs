using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.DataAccess.Repository.iRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category

        //required property calling first or default
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);

        //implement retrieving and displaying details from db
        IEnumerable<T> GetAll(string? includeProperties = null);

        //implement adding details to db
        void Add(T entity);

        //implement deleting details in db
        void Remove(T entity);

        //implement deleting range of details in db
        void RemoveRange(IEnumerable<T> entity);

    }
}
