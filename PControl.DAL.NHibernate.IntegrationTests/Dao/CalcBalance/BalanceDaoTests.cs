using System;
using System.Data;
using System.Linq;
using BIZ.PControl.DAL.NHibernate.IntegrationTests.Dao.Infrastructure;
using BIZ.PControl.DomainModel.Dao.CalcBalance;
using NUnit.Framework;

// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable MemberCanBePrivate.Global

namespace BIZ.PControl.DAL.NHibernate.IntegrationTests.Dao.CalcBalance
{
    [TestFixture]
    public class BalanceDaoTests : AbstractDaoIntegrationTests
    {
        protected override string NHibernateXML => "file://Dao.Infrastructure/NHibernateSQLite.xml";
        protected override string DbProviderXML => "file://Dao.Infrastructure/DbProviderSQLite.xml";

        public IBalanceDao BalanceDao { get; set; }

        [SetUp]
        public void SetUpTests()
        {
            DefaultRollback = true;
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Products (Name) values ('Товар1')");
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Products (Name) values ('Товар2')");
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Products (Name) values ('Товар3')");
        }

        [Test(Description = "При отсутствии остатков возвращается пустой список")]
        public void GetBalancesOnDateTest_IfBalancesNotExistsShouldReturnEmptyList()
        {
            int[] productsIds = {1, 2, 3};

            var result = BalanceDao.GetBalancesOnDate(DateTime.Today, productsIds).ToArray();
            
            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test(Description = "Для несуществующих товаров должен вернуться пустой список")]
        public void GetBalancesOnDateTest_IfProductsNotExistsShouldReturnEmptyList()
        {
            int[] productsIds = { 4, 5, 6 };

            var result = BalanceDao.GetBalancesOnDate(DateTime.Today, productsIds).ToArray();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test(Description = "Остатки на указанную дату существуют")]
        public void GetBalancesOnDateTest_BalancesOnDateExistsShouldReturnRealData()
        {
            int[] productsIds = { 1, 2, 3 };
            var onDate = new DateTime(2016, 5, 31);
            var dbParameters = AdoTemplate.CreateDbParameters();
            dbParameters.Add(onDate);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 1, 1)", dbParameters);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 2, 2)", dbParameters);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 3, 3)", dbParameters);

            var result = BalanceDao.GetBalancesOnDate(onDate, productsIds).ToArray();

            Assert.That(result[0].Quantity, Is.EqualTo(1));
            Assert.That(result[1].Quantity, Is.EqualTo(2));
            Assert.That(result[2].Quantity, Is.EqualTo(3));
        }

        [Test(Description = "Остатки по товарам существуют на дату больше указанной. Должен вернутся пустой список")]
        public void GetBalancesOnDateTest_BalancesExistsOnLaterDateShouldReturnEmptyList()
        {
            int[] productsIds = { 1, 2, 3 };
            var onDate = new DateTime(2016, 5, 31);
            var laterDate = new DateTime(2016, 6, 01);
            var dbParameters = AdoTemplate.CreateDbParameters();
            dbParameters.Add(laterDate);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 1, 1)", dbParameters);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 2, 2)", dbParameters);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 3, 3)", dbParameters);

            var result = BalanceDao.GetBalancesOnDate(onDate, productsIds).ToArray();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test(Description = "Остатки по товарам существуют на дату меньше указанной. Должны вернутся значения")]
        public void GetBalancesOnDateTest_BalancesExistsOnEarlierDateShouldReturnRealData()
        {
            int[] productsIds = { 1, 2, 3 };
            var onDate = new DateTime(2016, 5, 31);
            var earlierDate = new DateTime(2016, 5, 30);
            var dbParameters = AdoTemplate.CreateDbParameters();
            dbParameters.Add(earlierDate);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 1, 1)", dbParameters);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 2, 2)", dbParameters);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "insert into Balances (Date, Product_Id, Quantity) values (?, 3, 3)", dbParameters);

            var result = BalanceDao.GetBalancesOnDate(onDate, productsIds).ToArray();

            Assert.That(result[0].Quantity, Is.EqualTo(1));
            Assert.That(result[1].Quantity, Is.EqualTo(2));
            Assert.That(result[2].Quantity, Is.EqualTo(3));
        }
    }
}
