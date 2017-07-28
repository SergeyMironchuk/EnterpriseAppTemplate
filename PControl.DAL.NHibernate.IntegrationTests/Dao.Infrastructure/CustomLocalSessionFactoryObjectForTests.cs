using NHibernate.Cfg;

namespace BIZ.PControl.DAL.NHibernate.IntegrationTests.Dao.Infrastructure
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CustomLocalSessionFactoryObjectForTests : CustomLocalSessionFactoryObject
    {
        public static Configuration Config { get; private set; }

        protected override void PostProcessConfiguration(Configuration config)
        {
            base.PostProcessConfiguration(config);
            Config = config;
        }
    }
}