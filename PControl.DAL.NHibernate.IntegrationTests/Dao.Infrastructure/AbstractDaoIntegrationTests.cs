using System.IO;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Spring.Testing.NUnit;

namespace BIZ.PControl.DAL.NHibernate.IntegrationTests.Dao.Infrastructure
{
    public abstract class AbstractDaoIntegrationTests : AbstractTransactionalDbProviderSpringContextTests
    {
        private ISession _session;

        protected override string[] ConfigLocations => new[]
        {
            "assembly://PControl.DAL.NHibernate/BIZ.PControl.DAL.NHibernate.Dao/DbContext.xml",
            DbProviderXML,
            NHibernateXML,
            "assembly://PControl.DAL.NHibernate/BIZ.PControl.DAL.NHibernate.Dao.CalcBalance/Dao.xml"
        };

        protected abstract string NHibernateXML { get; }

        protected abstract string DbProviderXML { get; }

        public override void SetUp()
        {
            base.SetUp();
            OpenSession();
            var config = CustomLocalSessionFactoryObjectForTests.Config;
            var schemaExport = new SchemaExport(config);
            CreateDatabaseSchema(schemaExport);

        }

        [TearDown]
        public void TearDownTests()
        {
            _session.Close();
        }

        private void OpenSession()
        {
            var sessionFactory = applicationContext["NHibernateSessionFactory"];
            _session = ((ISessionFactory) sessionFactory).OpenSession();
        }

        private void CreateDatabaseSchema(SchemaExport schemaExport)
        {
            var stream = new NonClosedStream();
            var streamWriter = new StreamWriter(stream);
            schemaExport.Execute(false, true, false, _session.Connection, streamWriter);
            var streamReader = new StreamReader(stream);
            var schemaCreateCommandText = streamReader.ReadToEnd();
            streamReader.Close();
            stream.ForcedClose();
            AdoTemplate.Execute(command => command.CommandText = schemaCreateCommandText);
        }

        private class NonClosedStream : MemoryStream
        {
            public override void Close()
            {
                //base.Close();
            }

            public void ForcedClose() { base.Close(); }
        }
    }
}