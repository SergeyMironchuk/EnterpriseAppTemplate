using BIZ.PControl.DomainModel.CalcBalance;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BIZ.PControl.DAL.NHibernate.Mapping.CalcBalance
{
    public class DocumentMap: ClassMapping<Document>
    {
        public DocumentMap()
        {
            Table("Documents");
            Id(document => document.Id, mapper =>
            {
                mapper.Generator(Generators.Identity);
                mapper.Column("Id");
            });
            Property(document => document.Description, mapper => mapper.Column(c =>
            {
                c.Name("Description");
                c.Length(256);
            }));
            Property(document => document.Date, mapper => mapper.Column(c =>
            {
                c.Name("Date");
                //c.SqlType("datetime");
                c.NotNullable(true);
            }));
            Set(document => document.Details, mapper =>
            {
                mapper.Key(k => k.Column(columnMapper =>
                {
                    columnMapper.Name("Document_Id");
                }));
                mapper.Cascade(Cascade.All);
                mapper.Inverse(true);
            }, relation => relation.OneToMany());
        }
    }
}