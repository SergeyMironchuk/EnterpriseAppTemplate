using BIZ.PControl.DomainModel.CalcBalance;
using NHibernate.Mapping.ByCode.Conformist;

namespace BIZ.PControl.DAL.NHibernate.Mapping.CalcBalance
{
    public class BalanceMap : ClassMapping<Balance>
    {
        public BalanceMap()
        {
            Table("Balances");
            ComposedId(mapper =>
            {
                mapper.Property(balance => balance.Date,
                    propertyMapper => propertyMapper.Column(c =>
                    {
                        c.Name("Date");
                        //c.SqlType("datetime");
                        c.NotNullable(true);
                    }));
                mapper.ManyToOne(balance => balance.Product, oneMapper =>
                {
                    oneMapper.Column(c =>
                    {
                        c.Name("Product_Id");
                        //c.SqlType("int");
                        c.NotNullable(true);
                    });
                    oneMapper.ForeignKey("Balances_product");
                });
            });
            Property(balance => balance.Quantity, mapper => mapper.Column(c =>
            {
                c.Name("Quantity");
                //c.SqlType("float");
                c.NotNullable(true);
            }));
        }
    }
}