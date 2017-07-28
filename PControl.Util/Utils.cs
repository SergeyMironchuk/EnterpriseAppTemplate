using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using BIZ.PControl.DomainModel;
using BIZ.PControl.DomainModel.CalcBalance;
using BIZ.PControl.DomainModel.Dao;
using BIZ.PControl.DomainModel.Dao.CalcBalance;
using BIZ.PControl.DomainModel.Services;
using BIZ.PControl.DomainModel.Services.CalcBalance;

#pragma warning disable 649

namespace BIZ.PControl.Util
{
    public class Utils
    {

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private static int _productCount = 5000;
        private IProductDao _productDao;
        private static IBalanceDao _balanceDao;

        public void GenerateProducts()
        {
            for (int i = 0; i < _productCount; i++)
            {
                var product = new Product
                {
                    Name = $"Товар{i + 1}"
                };
                _productDao.Save(product);
            }
        }

        private static Document CreateRandomDocument()
        {
            var document = new Document
            {
                Date = DateTime.Today.AddDays(-10),
                Description = "Document"
            };
            for (int i = 0; i < 10; i++)
            {
                var detail = new DocumentDetail
                {
                    Product = new Product { Id = i + 1 },
                    Quantity = Random.NextDouble()
                };
                document.AddDetail(detail);
            }
            return document;
        }

        public void GenerateBalances(int productIdFrom, int productIdTo)
        {
            var stopWatch = Stopwatch.StartNew();
            long count = 0;
            for (int i = 125; i < 365; i++)
            {
                for (int j = productIdFrom; j <= productIdTo ; j++)
                {
                    var balance =new Balance
                    {
                        Date = DateTime.Today.AddDays(-i),
                        Product = new Product { Id = j },
                        Quantity = Random.NextDouble()
                    };
                    if (_balanceDao.Get(balance) == null) _balanceDao.Save(balance);
                    if (++count % 10000 == 0)
                    {
                        Console.WriteLine("Добавлено {1:N} записей. Затрачено времени {0}ms",
                            stopWatch.ElapsedMilliseconds,
                            count);
                        stopWatch.Reset();
                        stopWatch.Start();
                    }
                }
            }
        }

        public static void ListResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var res = assembly.GetManifestResourceNames();
            foreach (var s in res)
            {
                Console.WriteLine(s);
                using (Stream stream = assembly.GetManifestResourceStream(s))
                    if (stream != null)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string result = reader.ReadToEnd();
                            Console.WriteLine(result);
                        }
            }
        }

        private void AddDocument(IDocumentsManager documentsManager)
        {
            Stopwatch stopWatch = null;
            try
            {
                var document = CreateRandomDocument();
                stopWatch = Stopwatch.StartNew();
                documentsManager.AddDocument(document);
                stopWatch.Stop();
                Console.WriteLine("Документ N {1} добавлен. Затрачено времени {0}ms", stopWatch.ElapsedMilliseconds, document.Id);
            }
            catch (Exception e)
            {
                if (stopWatch != null)
                    Console.WriteLine(
                        $"Ошибка при добавлении документа. Затрачено времени {stopWatch.ElapsedMilliseconds}ms. Текст ошибки {e.Message}");
            }
            finally
            {
                stopWatch?.Stop();
            }
        }

        public void AddDocuments(int delayTime, int timesNumber, IDocumentsManager documentsManager)
        {
            var stopWatchTotal = Stopwatch.StartNew();
            for (int i = 0; i < timesNumber; i++)
            {
                AddDocument(documentsManager);
                if (i == 0) stopWatchTotal = Stopwatch.StartNew();
                stopWatchTotal.Stop();
                Thread.Sleep(delayTime);
                stopWatchTotal.Start();
            }
            stopWatchTotal.Stop();
            Console.WriteLine(
                $"Итоговый результат. Обработано {delayTime*timesNumber} документов. Затрачено времени {stopWatchTotal.ElapsedMilliseconds}ms");
        }
    }
}