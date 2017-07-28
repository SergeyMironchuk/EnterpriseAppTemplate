using System.Linq;
using BIZ.PControl.DomainModel.CalcBalance;
using BIZ.PControl.DomainModel.Dao.CalcBalance;
using Spring.Stereotype;
using Spring.Transaction.Interceptor;

namespace BIZ.PControl.DAL.NHibernate.Dao.CalcBalance
{
    [Repository]
    public class DocumentDao : HibernateDao, IDocumentDao
    {
        [Transaction(ReadOnly = true)]
        public Document Get(long id)
        {
            return CurrentSession.Get<Document>(id);
        }

        [Transaction(ReadOnly = true)]
        public IQueryable<Document> GetAll()
        {
            return GetAll<Document>();
        }

        [Transaction]
        public void Delete(Document entity)
        {
            CurrentSession.Delete(entity);
        }

        [Transaction]
        public long Save(Document entity)
        {
            return (long)CurrentSession.Save(entity);
        }

        [Transaction]
        public void Update(Document entity)
        {
            CurrentSession.Update(entity);
        }
    }
}