using Dapper;
using Eagle.Exam8.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Exam8.DataMigrationJob.Jobs
{
    /// <summary>
    /// TK_QuestionsBasic（大题表）表迁移服类
    /// </summary>
    public class DataMigrationOfTKQuestionsBasic : DataMigrationTemplate
    {
        private static readonly string _SourceTableName = "dbo.TK_QuestionsBasic";

        private static readonly string _TargetDbConnectionKey = "MigrationDb_XY_System2016";

        /// <summary>
        /// 存留数据天数，值为负数，例：-180天
        /// </summary>
        private static readonly int _RetainedDataDays = -180;

        /// <summary>
        /// 存留数据天数，值为负数，例：-180天
        /// </summary>
        public static int RetainedDataDays => _RetainedDataDays;

        /// <summary>
        /// 迁移数据的数据日期
        /// </summary>
        private readonly DateTime _DateOfMigratedData;

        /// <summary>
        /// 删除数据的数据日期
        /// </summary>
        private readonly DateTime _DateOfDeletedData;

        /// <summary>
        /// 默认迁移目标数据库名称
        /// </summary>
        private readonly string _TargetDbName;

        /// <summary>
        /// 目标数据库连接字符串
        /// </summary>
        private readonly string _TargetDbConnectionString;

        /// <summary>
        /// 迁移数据和删除数据筛选条件
        /// </summary>
        private readonly string _WhereSql;

        private readonly string _OrderBySql;

        /// <summary>
        /// 单次获取数据行数
        /// </summary>
        private readonly int _CopyBatchSize = 10000 * 10;

        /// <summary>
        /// 批量单次删除数据行数
        /// </summary>
        private readonly int _DeleteBatchSize = 1000 * 10;


        /// <summary>
        /// 私有构造函数
        /// </summary>
        private DataMigrationOfTKQuestionsBasic() : base(_SourceTableName)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dateOfDeleteData">迁移数据的日期</param>
        public DataMigrationOfTKQuestionsBasic(DateTime dateOfMigratedData) : base(_SourceTableName)
        {
            _DateOfMigratedData = dateOfMigratedData;
            _DateOfDeletedData = dateOfMigratedData;
            _TargetDbName = $"XY_System2016_{_DateOfMigratedData:yyyyMM}";
            _TargetDbConnectionString = string.Format(ConfigHelper.GetAppSettingValue(_TargetDbConnectionKey, true), _TargetDbName);
            _WhereSql = $" WHERE CreateDate BETWEEN '{_DateOfMigratedData:yyyy-MM-dd 00:00:00.000}' AND '{_DateOfMigratedData:yyyy-MM-dd 23:59:59.999}' AND IsGenerate > 0";
            _OrderBySql = " ORDER BY CreateDate ASC ";
            base.SetTargetDbName(_TargetDbName);
            base.SetDateOfMigratedData($"{_DateOfMigratedData:yyyy-MM-dd}");
            base.SetDateOfDeletedData($"{_DateOfDeletedData:yyyy-MM-dd}");
            base.SetMigratedFilterWhere(_WhereSql);
            base.SetDeletedFilterWhere(_WhereSql);
        }


        protected override int GetSourceDataCount()
        {
            var sql = $@" SELECT COUNT(*) FROM {_SourceTableName} WITH(NOLOCK) {_WhereSql}; ";
            using (var connection = new SqlConnection(ConnectionStrings.Exam8_SystemconnStr))
            {
                return connection.Query<int>(sql).FirstOrDefault();
            }
        }

        protected override int BulkCopyData()
        {
            //已完成迁移数据总数
            var migratedCount = 0;
            using (var dataReader = GetSourceData())
            {
                using (var connection = new SqlConnection(_TargetDbConnectionString))
                {
                    connection.Open();
                    migratedCount = connection.BulkCopy(
                        dataReader,
                        _SourceTableName,
                        60 * 30,
                        _CopyBatchSize
                    );
                }
                dataReader.Close();
            }
            return migratedCount;
        }

        protected override bool CheckMigratedData()
        {
            if (base.SourceDataCount == GetMigratedDataCount())
            {
                return true;
            }
            return false;
        }

        protected override int DeleteSourceData()
        {
            if (_DateOfMigratedData.Date > DateTime.Now.Date.AddDays(_RetainedDataDays))
            {
                return 0;
            }
            var deletedSourceCount = 0;
            int currentDeletedCount;
            do
            {
                currentDeletedCount = BatchDeleteSourceData();
                deletedSourceCount += currentDeletedCount;

            } while (currentDeletedCount > 0);

            return deletedSourceCount;
        }

        protected override bool CheckDeletedData()
        {
            if (base.SourceDataCount == base.DeletedSourceCount)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取读取源数据 IDataReader 对象
        /// </summary>
        /// <returns>
        /// 返回：IDataReader 实例
        /// </returns>
        private IDataReader GetSourceData()
        {
            var querySql = $@"
                WITH tempTable 
                AS ( SELECT * FROM {_SourceTableName} WITH(NOLOCK) {_WhereSql} ) 
                SELECT * FROM tempTable WITH(NOLOCK) 
                {_OrderBySql};
            ";
            return SqlHelper.ExecuteReader(ConnectionStrings.Exam8_SystemconnStr,
                CommandType.Text,
                querySql,
                null);
        }

        /// <summary>
        /// 获取已迁移数据总数
        /// </summary>
        /// <returns>
        /// 返回：目标库表迁移成功数据总数
        /// </returns>
        private int GetMigratedDataCount()
        {
            var sql = $@" SELECT COUNT(*) FROM {_SourceTableName} WITH(NOLOCK) {_WhereSql}; ";
            using (var connection = new SqlConnection(_TargetDbConnectionString))
            {
                return connection.Query<int>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <returns></returns>
        private int BatchDeleteSourceData()
        {
            var deleteSql = $@"
                BEGIN
                    DECLARE @DELETE_SUM_ROWS INT = 0;
                    DECLARE @DELETE_CURRENT_ROWS INT = 0;
                    WHILE 1 = 1
                    BEGIN
                        DELETE TOP (2000)
                        FROM {_SourceTableName} {_WhereSql};
		                SET @DELETE_CURRENT_ROWS = @@ROWCOUNT;
                        SET @DELETE_SUM_ROWS += @DELETE_CURRENT_ROWS
		                IF @DELETE_CURRENT_ROWS < 2000
			                BREAK;
                        IF @DELETE_SUM_ROWS >= {_DeleteBatchSize}  
                            BREAK;
                        IF @@ERROR <> 0
                        BEGIN
                            SET @DELETE_SUM_ROWS = -1;
                            BREAK;
                        END;
                    END;
                END;
                SELECT @DELETE_SUM_ROWS;
            ";
            using (var connection = new SqlConnection(ConnectionStrings.Exam8_SystemconnStr))
            {
                return connection.ExecuteScalar<int>(deleteSql, null, null, 60 * 30);
            }
        }
    }
}
