using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataGenies.Core.Tests.Integration.Stubs.Data
{
    public class InMemoryDbSet<T> : Microsoft.EntityFrameworkCore.DbSet<T>, IQueryable<T> where T : class
    {
        readonly ObservableCollection<T> _data;
        readonly IQueryable _query;

        public InMemoryDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public new virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public new virtual T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public new T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public new T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public new ObservableCollection<T> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}