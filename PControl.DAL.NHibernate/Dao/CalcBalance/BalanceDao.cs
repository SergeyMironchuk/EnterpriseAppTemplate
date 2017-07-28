using System;
using System.Collections.Generic;
using System.Linq;
using BIZ.PControl.DomainModel.CalcBalance;
using BIZ.PControl.DomainModel.Dao.CalcBalance;
using Spring.Stereotype;
using Spring.Transaction.Interceptor;

namespace BIZ.PControl.DAL.NHibernate.Dao.CalcBalance
{
    [Repository]
    public class BalanceDao : HibernateDao, IBalanceDao
    {
        [Transaction(ReadOnly = true)]
        public IEnumerable<Balance> GetBalancesOnDate(DateTime onDate, int[] productIds)
        {
            var multiQuery = CurrentSession.CreateMultiQuery();
            foreach (int id in productIds)
            {
                multiQuery.Add<Balance>(
                    CurrentSession.CreateQuery(@"
                        from Balance 
                        where Date = (select max(b.Date) from Balance as b where b.Date <= :onDate and b.Product.Id = :productId)
                            and Product.Id = :productId
                    ")
                        .SetDateTime("onDate", onDate)
                        .SetInt32("productId", id)
                    );
            }
            var balancesOnDate = multiQuery.List().Cast<List<Balance>>();
            return (from balance in balancesOnDate select balance.Count == 0 ? null : balance[0]).Where(b => b != null).ToList();
        }

        [Transaction]
        public void UpdateBalancesFromDate(DateTime fromDate, int productId, double addedQuantity)
        {
            var update = CurrentSession.CreateQuery("update Balance set Quantity = Quantity + :addedQuantity where Date >= :fromDate and Product.Id = :productId")
                    .SetDouble("addedQuantity", addedQuantity)
                    .SetDateTime("fromDate", fromDate)
                    .SetInt32("productId", productId);
            update.ExecuteUpdate();
        }

        [Transaction(ReadOnly = true)]
        public Balance Get(object id)
        {
            return CurrentSession.Get<Balance>(id);
        }

        [Transaction(ReadOnly = true)]
        public IQueryable<Balance> GetAll()
        {
            return GetAll<Balance>();
        }

        [Transaction]
        public void Delete(Balance entity)
        {
            CurrentSession.Delete(entity);
        }

        [Transaction]
        public Balance Save(Balance entity)
        {
            return (Balance)CurrentSession.Save(entity);
        }

        [Transaction]
        public void Update(Balance entity)
        {
            CurrentSession.Update(entity);
        }
    }
}