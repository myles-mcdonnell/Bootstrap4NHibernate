using System;
using NHibernate;

namespace Bootstrap4NHibernate
{
    public class Session : Data.ISession, IDisposable
    {
        private readonly ISession _session;

        public Session(ISession session)
        {
            _session = session;
        }

        public T Save<T>(T entity)
        {
            return (T)_session.Save(entity);
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        public ITransaction BeginTransaction()
        {
            return _session.BeginTransaction();
        }
    }
}
