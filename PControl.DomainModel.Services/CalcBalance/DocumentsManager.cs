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
                var productBalanceOnDate = balancesOnDate.FirstOrDefault(b => b.Product.Id == detail.Product.Id);
                if (productBalanceOnDate == null || productBalanceOnDate.Date < document.Date)
                {
                    var newBalance = new Balance
                    {
                        Date = document.Date,
                        Product = detail.Product,
                        Quantity = productBalanceOnDate?.Quantity ?? 0
                    };
                    BalanceDao.Save(newBalance);
                }
            }
        }
    }
}