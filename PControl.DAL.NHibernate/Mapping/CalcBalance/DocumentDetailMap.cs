using BIZ.PControl.DomainModel.CalcBalance;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BIZ.PControl.DAL.NHibernate.Mapping.CalcBalance
{
    public class DocumentDetailMap: ClassMapping<DocumentDetail>
    {
        public DocumentDetailMap()
        {
            Table("DocumentsDetails");
            Id(detail => detail.Id, mapper =>
            {
                mapper.Generator(Generators.Identity);
                mapper.Column("Id");
            });
            ManyToOne(detail => detail.Document, mapper =>
            {
                mapper.Column(c =>
                {
                    c.Name("Document_Id");
                    //c.SqlType("bigint");
                    c.NotNullable(true);
                });
                mapper.ForeignKey("DocumentsDetails_Documents");
            });
            ManyToOne(detail => detail.Product, mapper =>
            {
                mapper.Column(c =>
                {
                    c.Name("Product_Id");
                    //c.SqlType("int");
                    c.NotNullable(true);
                });
                mapper.ForeignKey("DocumentsDetails_Products");
            });
            Property(detail => detail.Quantity, mapper => mapper.Column(c =>
            {
                c.Name("Quantity");
                //c.SqlType("float");
                c.NotNullable(true);
            }));
        }
    }
}