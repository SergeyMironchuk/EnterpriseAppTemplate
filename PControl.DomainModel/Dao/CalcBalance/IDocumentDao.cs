using BIZ.PControl.DomainModel.CalcBalance;

namespace BIZ.PControl.DomainModel.Dao.CalcBalance
{
    public interface IDocumentDao : IDao<Document, long>, ISupportsDeleteDao<Document>, ISupportsSave<Document, long>
    {
    }
}