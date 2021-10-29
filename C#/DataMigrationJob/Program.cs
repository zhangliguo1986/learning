using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Exam8.DataMigrationJob.Jobs;
using System.Globalization;

namespace Eagle.Exam8.DataMigrationJob
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log.Info($"---------------------------【DataMigrationJob】- Job开始执行 ---------------------------");


            DateTime? inputDate = null;
            if (args != null && args.Length > 0)
            {
                var arg0 = args[0];
                if (DateTime.TryParseExact(arg0, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime date))
                {
                    inputDate = date;
                }
                log.Info($"【DataMigrationJob】- 获取指定日期【{arg0}】");
            }
            else
            {
                //2021-10-27日执行，从2021-02-02日开始迁移数据
                //inputDate = DateTime.Now.AddDays(-267);
            }
            //数据迁移服务 - 启动
            new DataMigrationService().Start(inputDate);


            log.Info($"===========================【DataMigrationJob】- Job执行完毕 ===========================");
            Console.WriteLine("执行完毕....");
            //Console.ReadLine();
        }
    }
}
