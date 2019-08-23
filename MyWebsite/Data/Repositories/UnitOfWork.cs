using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PortfolioWebsite.Data.Interfaces;

namespace PortfolioWebsite.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork   /*IUnitOfWork<T> where T : class*/
    {
        protected readonly ApplicationDbContext _context;

        public IBudgetRepository BudgetRepository { get; set; }
        public IBudgetItemsRepository BudgetItemsRepository { get; set; }
        public IWeatherRepository WeatherRepository { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            BudgetRepository = new BudgetRepository(context);
            BudgetItemsRepository = new BudgetItemsRepository(context);
            WeatherRepository = new WeatherRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
        //public void Save() => _context.SaveChanges();

        //public int Count(Func<T, bool> predicate)
        //{
        //   return _context.Set<T>().Where(predicate).Count();
        //}

        //public void Create(T entity)
        //{
        //    _context.Add(entity);
        //    Save();
        //}

        //public void Delete(T entity)
        //{
        //    _context.Remove(entity);
        //    Save();
        //}

        //public IEnumerable<T> Find(Func<T, bool> predicate)
        //{
        //    return _context.Set<T>().Where(predicate);
        //}

        //public IEnumerable<T> GetAll()
        //{
        //    return _context.Set<T>();
        //}

        //public T GetById(int id)
        //{
        //    return _context.Set<T>().Find(id);
        //}

        //public void Update(T entity)
        //{
        //    _context.Entry(entity).State = EntityState.Modified;
        //}
    }
}
