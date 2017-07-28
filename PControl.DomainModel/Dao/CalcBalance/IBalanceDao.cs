using System;
using System.Collections.Generic;
using BIZ.PControl.DomainModel.CalcBalance;

namespace BIZ.PControl.DomainModel.Dao.CalcBalance
{
    public interface IBalanceDao : IDao<Balance, object>, ISupportsDeleteDao<Balance>, ISupportsSave<Balance, Balance>
    {
        IEnumerable<Balance> GetBalancesOnDate(DateTime onDate, int[] productIds);
        void UpdateBalancesFromDate(DateTime fromDate, int productId, double addedQuantity);
    }
}