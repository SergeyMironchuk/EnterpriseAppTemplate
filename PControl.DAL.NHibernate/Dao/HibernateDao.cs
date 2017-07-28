using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace BIZ.PControl.DAL.NHibernate.Dao
{
    /// <summary>
    /// Base class for data access operations.
    /// </summary>
    public abstract class HibernateDao
    {
        private ISessionFactory _sessionFactory;

        /// <summary>
        /// Session factory for sub-classes.
        /// </summary>
        public ISessionFactory SessionFactory
        {
            protected get { return _sessionFactory; }
            set { _sessionFactory = value; }
        }

        /// <summary>
        /// Get's the current active session. Will retrieve session as managed by the 
        /// Open Session In View module if enabled.
        /// </summary>
        protected ISession CurrentSession
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        protected IQueryable<T> GetAll<T>() where T : class
        {
            return CurrentSession.Query<T>();
        }
    }
}