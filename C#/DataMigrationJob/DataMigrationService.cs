using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Eagle.Exam8.Utils;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Eagle.Exam8.DataMigrationJob.Jobs;

namespace Eagle.Exam8.DataMigrationJob
{
    /// <summary>
    /// 数据迁移服务类
    /// </summary>
    public class DataMigrationService
    {
        private readonly ILog _Log = LogManager.GetLogger(typeof(DataMigrationService));

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="inputDate"></param>
        public void Start(DateTime? inputDate)
        {
            _Log.Info("【DataMigrationService】======> 服务启动");

            DateTime migratingDate = DateTime.Now.AddDays(DataMigrationOfTKUserExamPaper.RetainedDataDays);
            if(inputDate != null)
            {
                migratingDate = inputDate.Value;
            }

            //TK_QuestionsBasic（大题表）表--迁移服务（不能按时间删除）
            var migrateQuestionsBasic = new DataMigrationOfTKQuestionsBasic(migratingDate);
            migrateQuestionsBasic.StartDataMigration();

            //TK_ExamPaperBasic（试卷表）表--迁移服务（不能按时间删除）
            var migratePaperBasic = new DataMigrationOfTKExamPaperBasic(migratingDate);
            migratePaperBasic.StartDataMigration();

            //TK_UserExamPaper（用户试卷表） 表--迁移服务
            var migrateUserExamPaper = new DataMigrationOfTKUserExamPaper(migratingDate);
            migrateUserExamPaper.StartDataMigration();

            //TK_UserAnswer（用户答题表） 表--迁移服务
            var migrateUserAnswer = new DataMigrationOfTKUserAnswer(migratingDate);
            migrateUserAnswer.StartDataMigration();

            //TK_RandomQuestions（随机试题表）表--迁移服务
            var migrateRandomQuestion = new DataMigrationOfTKRandomQuestions(migratingDate);
            migrateRandomQuestion.StartDataMigration();


            _Log.Info("【DataMigrationService】======> 服务停止");
        }
    }
}
