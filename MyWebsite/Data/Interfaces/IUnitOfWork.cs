using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IBudgetRepository BudgetRepository { get; set; }
        IBudgetItemsRepository BudgetItemsRepository { get; set; }
        IWeatherRepository WeatherRepository { get; set; }

        void Complete();
        //int Count(Func<T, bool> predicate);
        //void Create(T entity);
        //void Delete(T entity);
        //IEnumerable<T> Find(Func<T, bool> predicate);
        //IEnumerable<T> GetAll();
        //T GetById(int id);
        //void Update(T entity);
    }
}
