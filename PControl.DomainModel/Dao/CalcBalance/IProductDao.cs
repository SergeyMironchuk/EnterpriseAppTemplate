using BIZ.PControl.DomainModel.CalcBalance;

namespace BIZ.PControl.DomainModel.Dao.CalcBalance
{
    public interface IProductDao : IDao<Product, int>, ISupportsDeleteDao<Product>, ISupportsSave<Product, int>
    {
    }
}