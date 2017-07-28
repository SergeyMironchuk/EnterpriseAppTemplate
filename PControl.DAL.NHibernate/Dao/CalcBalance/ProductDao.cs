using System.Linq;
using BIZ.PControl.DomainModel.CalcBalance;
using BIZ.PControl.DomainModel.Dao.CalcBalance;
using Spring.Stereotype;
using Spring.Transaction.Interceptor;

namespace BIZ.PControl.DAL.NHibernate.Dao.CalcBalance
{
    [Repository]
    public class ProductDao : HibernateDao, IProductDao
    {
        [Transaction(ReadOnly = true)]
        public Product Get(int id)
        {
            return CurrentSession.Get<Product>(id);
        }

        [Transaction(ReadOnly = true)]
        public IQueryable<Product> GetAll()
        {
            return GetAll<Product>();
        }

        [Transaction]
        public void Delete(Product product)
        {
            CurrentSession.Delete(product);
        }

        [Transaction]
        public int Save(Product product)
        {
            return (int)CurrentSession.Save(product);
        }

        [Transaction]
        public void Update(Product product)
        {
            CurrentSession.SaveOrUpdate(product);
        }
    }
}