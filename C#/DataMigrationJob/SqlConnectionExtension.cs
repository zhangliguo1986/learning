using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Eagle.Exam8.DataMigrationJob
{
    public static class SqlConnectionExtension
    {
        private static FieldInfo _RowsCopiedField = null;


        public static int BulkCopy(this SqlConnection connection,
            DataTable dtSource,
            string tableName,
            int bulkCopyTimeout = 30,
            int batchSize = 0,
            SqlBulkCopyOptions options = SqlBulkCopyOptions.Default,
            SqlTransaction transaction = null
            )
        {
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connection, options, transaction))
            {
                //目标表表名
                sqlbulkcopy.DestinationTableName = tableName;
                //每批次处理行数
                sqlbulkcopy.BatchSize = batchSize;
                //超时时间
                sqlbulkcopy.BulkCopyTimeout = bulkCopyTimeout;
                //创建字段映射
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    sqlbulkcopy.ColumnMappings.Add(dtSource.Columns[i].ColumnName, dtSource.Columns[i].ColumnName);
                }
                sqlbulkcopy.WriteToServer(dtSource);
                //返回批量拷贝行数
                return dtSource.Rows.Count;
            }
        }

        public static async Task<int> BullCopyAsync(this SqlConnection connection,
            DataTable dtSource,
            string tableName,
            int bulkCopyTimeout = 30,
            int batchSize = 0,
            SqlBulkCopyOptions options = SqlBulkCopyOptions.Default,
            SqlTransaction transaction = null
            )
        {
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connection, options, transaction))
            {
                try
                {
                    //目标表表名
                    sqlbulkcopy.DestinationTableName = tableName;
                    //每批次处理行数
                    sqlbulkcopy.BatchSize = batchSize;
                    //超时时间
                    sqlbulkcopy.BulkCopyTimeout = bulkCopyTimeout;
                    //创建字段映射
                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        sqlbulkcopy.ColumnMappings.Add(dtSource.Columns[i].ColumnName, dtSource.Columns[i].ColumnName);
                    }
                    await sqlbulkcopy.WriteToServerAsync(dtSource);
                    //返回批量拷贝行数
                    return dtSource.Rows.Count;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static int BulkCopy(this SqlConnection connection,
            IDataReader dataReader,
            string tableName,
            int bulkCopyTimeout = 30,
            int batchSize = 0,
            SqlBulkCopyOptions options = SqlBulkCopyOptions.Default,
            SqlTransaction transaction = null
            )
        {
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connection, options, transaction))
            {
                //目标表表名
                sqlbulkcopy.DestinationTableName = tableName;
                //每批次处理行数
                sqlbulkcopy.BatchSize = batchSize;
                //超时时间
                sqlbulkcopy.BulkCopyTimeout = bulkCopyTimeout;
                //启用流式传输数据
                sqlbulkcopy.EnableStreaming = true;
                //批量写入
                sqlbulkcopy.WriteToServer(dataReader);

                _RowsCopiedField = typeof(SqlBulkCopy).GetField("_rowsCopied", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                //获取批量拷贝行数
                var count = (int)_RowsCopiedField.GetValue(sqlbulkcopy);
                return count;
            }
        }

    }
}

