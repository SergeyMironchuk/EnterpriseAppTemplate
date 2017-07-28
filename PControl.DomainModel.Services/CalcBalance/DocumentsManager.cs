using System.Data;
using System.Linq;
using BIZ.PControl.DomainModel.CalcBalance;
using BIZ.PControl.DomainModel.Dao.CalcBalance;
using Spring.Transaction.Interceptor;

// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable MemberCanBePrivate.Global

namespace BIZ.PControl.DomainModel.Services.CalcBalance
{
    public class DocumentsManager: IDocumentsManager
    {
        public IBalanceDao BalanceDao { get; set; }
        public IDocumentDao DocumentDao { get; set; }

        [Transaction(IsolationLevel.RepeatableRead)]
        public void AddDocument(Document document)
        {
            AddNewBalancesIfNeed(document);
            UpdateExistsBalances(document);
            DocumentDao.Save(document);
        }

        private void UpdateExistsBalances(Document document)
        {
            foreach (var detail in document.Details)
            {
                BalanceDao.UpdateBalancesFromDate(document.Date, detail.Product.Id, detail.Quantity);
            }
        }

        private void AddNewBalancesIfNeed(Document document)
        {
            var productIds = document.Details.Select(d => d.Product.Id).ToArray();
            var balancesOnDate = BalanceDao.GetBalancesOnDate(document.Date, productIds).ToList();
            foreach (var detail in document.Details)
            {
                var balanceOnDate =
                    balancesOnDate.FirstOrDefault(b => b.Date == document.Date && b.Product.Id == detail.Product.Id);
                if (balanceOnDate == null || balanceOnDate.Date < document.Date)
                {
                    var newBalance = new Balance
                    {
                        Date = document.Date,
                        Product = detail.Product,
                        Quantity = balanceOnDate == null ? 0 : balanceOnDate.Quantity
                    };
                    BalanceDao.Save(newBalance);
                }
            }
        }
    }
}