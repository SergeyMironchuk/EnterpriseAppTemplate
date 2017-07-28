using System;
using System.Data;
using System.Linq;
using BIZ.PControl.DAL.NHibernate.IntegrationTests.Dao.Infrastructure;
using BIZ.PControl.DomainModel.CalcBalance;
using BIZ.PControl.DomainModel.Dao.CalcBalance;
using NUnit.Framework;

// ReSharper disable All

namespace BIZ.PControl.DAL.NHibernate.IntegrationTests
{

    [TestFixture]
    public class TrainingTests : AbstractDaoIntegrationTests
    {
        protected override string NHibernateXML { get { return "file://Dao.Infrastructure/NHibernateLocalDB.xml"; } }
        protected override string DbProviderXML { get { return "file://Dao.Infrastructure/DbProviderLocalDB.xml"; } }

        public IProductDao ProductDao { get; set; }

        [SetUp]
        public void SetUpTests()
        {
            ExecuteSqlScript("FillData.sql", false);
            DefaultRollback = true;
        }

        [Test]
        public void SimpleTest()
        {
            var rows = AdoTemplate.ExecuteScalar(CommandType.Text, "select count(*) from Products");
            Console.WriteLine("rows = {0}", rows);

            Assert.That(ProductDao, !Is.Null);

            foreach (var product in ProductDao.GetAll())
            {
                Console.WriteLine(product.Name);
            }
        }

        [Test]
        public void AdoTemplateMappingTest()
        {
            var products = AdoTemplate.QueryWithRowMapperDelegate(CommandType.Text, "select * from Products",
                (reader, num) => CreateProduct(reader)).Cast<Product>();

            foreach (var product in products)
            {
                Console.WriteLine(product.Name);
            }
        }

        private static Product CreateProduct(IDataReader reader)
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }
    }
}