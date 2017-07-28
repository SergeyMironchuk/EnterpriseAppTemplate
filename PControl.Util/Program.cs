using System;
using System.Configuration;
using System.Text;
using BIZ.PControl.DbSchema;
using BIZ.PControl.DomainModel.Services.CalcBalance;
using Spring.Context;
using Spring.Context.Support;

namespace BIZ.PControl.Util
{
    class Program
    {
        /// <summary>
        /// Утилита запуска процедуры добавления документов с пересчетом баланса.
        /// 
        /// Описание:
        /// В базу данных добавляются случайные документы с пересчетом остатков.
        /// Процесс повторяется указанное количество раз.
        /// Результаты работы протоколируются на консоль.
        /// 
        /// Формат командной строки:
        ///     CalcBalances [mode [times [delayTime]]]
        /// 
        /// Параметры командной строки:
        ///     mode - Режим работы
        ///         createdatabase - создание базы данных с товарами и начальными значениями остатков
        ///         process - добавление документов (режим по умолчанию)
        ///     times - количество раз запуска процедур (по умолчанию 1)
        ///     delayTime - время задержки в мс между запусками (по умолчанию 1мс)
        /// 
        /// Примеры командной строки:
        ///     CalcBalances createDatabase
        ///     CalcBalances process 1000 10000
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length > 0) Console.OutputEncoding = Encoding.GetEncoding(1251);

            var mode = "process";
            var timesNumber = 10;
            var delayTime = 1;
            if (args.Length > 0) mode = args[0].ToLower();
            if (args.Length > 1) timesNumber = Int32.Parse(args[1]);
            if (args.Length > 2) delayTime = Int32.Parse(args[2]);

            IApplicationContext applicationContext = ContextRegistry.GetContext();

            IDocumentsManager documentsManager = (IDocumentsManager)applicationContext["DocumentsManager"];
            Utils utils = (Utils)applicationContext["Utils"];
            switch (mode)
            {
                case "createdatabase":
                    Runner.MigrateToLatest(ConfigurationManager.ConnectionStrings["PControlConnectionString"].ConnectionString);
                    utils.GenerateProducts();
                    utils.GenerateBalances(1, 100);
                    return;
                default:
                    utils.AddDocuments(delayTime, timesNumber, documentsManager);
                    break;
            }
        }
    }
}
