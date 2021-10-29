using Eagle.Exam8.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Diagnostics;
using Eagle.Exam8.Utils.DingTalk;
using System.Threading;

namespace Eagle.Exam8.DataMigrationJob
{
    /// <summary>
    /// 数据迁移模板类（迁移表服务继承此类，重写方法，执行模板方法）
    /// </summary>
    public abstract class DataMigrationTemplate
    {
        private readonly ILog _Log = LogManager.GetLogger(typeof(DataMigrationTemplate));

        /// <summary>
        /// 本次待迁移数据总数, 初始值-1
        /// </summary>
        private int _SourceDataCount = -1;

        /// <summary>
        /// 本次完成迁移数据总数
        /// </summary>
        private int _MigratedCount = 0;

        /// <summary>
        /// 本次迁移数据是否成功
        /// </summary>
        private bool _IsMigratedSuccess = false;

        /// <summary>
        /// 本次删除数据总数
        /// </summary>
        private int _DeletedSourceCount = 0;

        /// <summary>
        /// 本次删除数据是否成功
        /// </summary>
        private bool _IsDeletedSuccess = false;

        protected int SourceDataCount => _SourceDataCount;

        protected int MigratedCount => _MigratedCount;

        protected int DeletedSourceCount => _DeletedSourceCount;

        /// <summary>
        /// 源表表名
        /// </summary>
        private readonly string _SourceTableName;

        /// <summary>
        /// 迁移目标数据库名称
        /// </summary>
        private string _TargetDbName;

        private string _DateOfMigratedData;     //迁移的数据日期
        private string _DateOfDeletedData;      //删除的数据日期
        private string _MigratedFilterWhere;    //迁移的数据条件
        private string _DeletedFilterWhere;     //删除的数据条件

        /// <summary>
        /// 开始时间
        /// </summary>
        private readonly DateTime _BeginTime;

        /// <summary>
        /// 模板类构造函数
        /// </summary>
        /// <param name="sourceTableName"></param>
        protected DataMigrationTemplate(string sourceTableName)
        {
            _SourceTableName = sourceTableName;
            _BeginTime = DateTime.Now;
        }

        protected void SetTargetDbName(string targetDbName)
        {
            _TargetDbName = targetDbName;
            _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 设置迁移目标数据库：{_TargetDbName}");
        }

        protected void SetDateOfMigratedData(string dateOfMigratedData)
        {
            _DateOfMigratedData = dateOfMigratedData;
            _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 设置迁移的数据日期：{_DateOfMigratedData}");
        }

        protected void SetDateOfDeletedData(string dateOfDeletedData)
        {
            _DateOfDeletedData = dateOfDeletedData;
            _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 设置删除的数据日期：{_DateOfDeletedData}");
        }

        protected void SetMigratedFilterWhere(string filterWhere)
        {
            _MigratedFilterWhere = filterWhere;
        }
        protected void SetDeletedFilterWhere(string filterWhere)
        {
            _DeletedFilterWhere = filterWhere;
        }


        /// <summary>
        /// 开始数据迁移 - 模板方法
        /// </summary>
        public void StartDataMigration()
        {
            _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 迁移数据【开始】，开始时间：{DateTime.Now: yyyy-MM-dd HH:mm:ss:fff}");

            string errMsg = null;
            try
            {
                if (_Log.IsDebugEnabled)
                {
                    //第一步：获取源表待迁移数据总数
                    Stopwatch sw = Stopwatch.StartNew();
                    _SourceDataCount = GetSourceDataCount();
                    sw.Stop();
                    _Log.Debug($"数据迁移服务 --【{_SourceTableName}表】- 获取源表待迁移数据总数：【{_SourceDataCount}】，耗时：{sw.Elapsed.TotalMilliseconds} 毫秒");

                    //第二步：迁移数据
                    sw.Restart();
                    _MigratedCount = _SourceDataCount > 0
                        ? BulkCopyData()
                        : 0;
                    sw.Stop();
                    _Log.Debug($"数据迁移服务 --【{_SourceTableName}表】- 结束拷贝数据，拷贝数据总数：【{_MigratedCount}】，耗时：{sw.Elapsed.TotalMilliseconds} 毫秒");

                    //第三步：校验数据后删除源数据
                    if (_SourceDataCount == _MigratedCount
                        && CheckMigratedData())
                    {
                        _IsMigratedSuccess = true;
                        sw.Restart();
                        //删除源表已迁移数据
                        _DeletedSourceCount = DeleteSourceData();
                        sw.Stop();
                        _Log.Debug($"数据迁移服务 --【{_SourceTableName}表】- 结束删除数据，删除数据总数：【{_DeletedSourceCount}】，耗时：{sw.Elapsed.TotalMilliseconds} 毫秒");
                    }
                }
                else
                {
                    //第一步：获取源表待迁移数据总数
                    _SourceDataCount = GetSourceDataCount();
                    _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 获取源表待迁移数据总数，待迁移总数：【{_SourceDataCount}】");

                    //第二步：迁移数据
                    _MigratedCount = _SourceDataCount > 0
                        ? BulkCopyData()
                        : 0;
                    _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 结束拷贝数据，拷贝数据总数：【{_MigratedCount}】");

                    //第三步：校验数据后删除源数据
                    if (_SourceDataCount == _MigratedCount
                        && CheckMigratedData())
                    {
                        _IsMigratedSuccess = true;
                        //删除源表已迁移数据
                        _DeletedSourceCount = DeleteSourceData();
                        _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 结束删除数据，删除数据总数：【{_DeletedSourceCount}】");
                    }
                }

                if (CheckDeletedData())
                {
                    //校验待删除数据总数与已删除总数是否一直
                    _IsDeletedSuccess = true;
                }

                //第四步：插入迁移记录表
                InserMigrationRecord();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 执行【出现异常】，迁移终止，错误：{ex.Message}");
                _Log.Error($"数据迁移服务 --【{_SourceTableName}表】- 执行【出现异常】，错误：{ex.Message}，堆栈：{ex.StackTrace}", ex);
            }

            //发送失败通知
            //if (!_IsMigratedSuccess || !_IsDeletedSuccess)
            //{
            //    Task.Run(() => SendNotice(errMsg));
            //}

            Task.Run(() => SendNotice(errMsg));
            _Log.Info($"数据迁移服务 --【{_SourceTableName}表】- 迁移数据【结束】，结束时间：{DateTime.Now: yyyy-MM-dd HH:mm:ss:fff}");

            Thread.Sleep(1000 * 60);
        }

        /// <summary>
        /// 获取本次源表待迁移数据总数
        /// </summary>
        /// <returns></returns>
        protected abstract int GetSourceDataCount();

        /// <summary>
        /// 迁移数据
        /// </summary>
        /// <returns>
        /// 返回：迁移的数据行数
        /// </returns>
        protected abstract int BulkCopyData();

        /// <summary>
        /// 校验数据迁移是否成功（判断待迁移的数据与已迁移的数据行数是否一致）
        /// </summary>
        /// <returns>
        /// 返回：bool，true：迁移成功，false：迁移失败
        /// </returns>
        protected abstract bool CheckMigratedData();

        /// <summary>
        /// 删除数据（迁移数据后执行的删除数据操作）
        /// </summary>
        /// <returns>
        /// 返回：删除的数据行数
        /// </returns>
        /// <remarks>
        /// 说明：继承类重写后，可根据业务删除指定条件的数据
        /// </remarks>
        protected abstract int DeleteSourceData();

        /// <summary>
        /// 校验数据删除是否成功（判断待删除的数据与已删除的数据行数是否一致）
        /// </summary>
        /// <returns>
        /// 返回：bool， true：删除成功，false：删除失败
        /// </returns>
        protected abstract bool CheckDeletedData();

        /// <summary>
        /// 发送迁移失败通知，邮件、SMS、钉钉等
        /// </summary>
        protected virtual void SendNoticeByEmail()
        {
            //TODO: 发送通知
            MailHelper.SendEmail("smtp.exmail.qq.com",
                    "发送Email",
                    "发送Email密码/秘钥",
                    "接收Email",
                    "【万题库 - 数据迁移Job通知】",
                    $@"数据迁移：{_IsMigratedSuccess.ToString()}，
                    迁移数据总数：{_MigratedCount}，
                    数据删除：{_IsDeletedSuccess}，
                    删除数据总数：{_DeletedSourceCount}",
                    false);
        }

        /// <summary>
        /// 发送迁移失败通知，邮件、SMS、钉钉等
        /// </summary>
        protected virtual void SendNotice(string errMsg)
        {
            //洛阳战队钉钉群地址和秘钥
            string dingTalkWebhook = "https://oapi.dingtalk.com/robot/send?access_token=e3edfdb412b78d1c38d99447b65f9b180d4e61b115d5b6bb3228b019e5f11be8";
            string dingTalksecret = "SECeeaeef10702fa9e0a8a7b5be9506d391bbee8da2ee953a824dda54c31cb6f7fe";

            var migratedResult = _IsMigratedSuccess ? "成功" : "失败";
            var deletedResult = _IsDeletedSuccess ? "成功" : "失败";
            //Markdown 格式消息通知
            var sb = new StringBuilder();
            sb.AppendLine("#### **【万题库-数据迁移Job】- 业务告警**");
            sb.AppendLine("--- ");
            sb.AppendLine($"- **【告警信息】：**【{_SourceTableName}】表，迁移：***{migratedResult}***，迁移总行数：***{_MigratedCount}***，删除：***{deletedResult}***，删除总行数：***{_DeletedSourceCount}***  。");
            sb.AppendFormat("- **【异常详情】：** {0}", !string.IsNullOrEmpty(errMsg) ? errMsg : "【无】");
            var msg = new DingTalkMsgEntity
            {
                msgtype = MsgTypeEnum.markdown,
                markdown = new DingTalkMsgForMarkdown()
                {
                    title = "业务告警",
                    text = sb.ToString(),
                    isAtAll = true
                }
            };
            DingTalkPush.PushMessage(msg, dingTalkWebhook, dingTalksecret);
        }


        /// <summary>
        /// 插入数据迁移记录
        /// </summary>
        protected virtual void InserMigrationRecord()
        {
            var sql = @"
                INSERT INTO [dbo].[DataMigrationRecord]
                       ([TableName]
                       ,[TargetDbName]
                       ,[SourceDataCount]
                       ,[MigratedCount]
                       ,[DateOfMigratedData]
                       ,[IsMigratedSuccess]
                       ,[DeletedSourceCount]
                       ,[DateOfDeletedData]
                       ,[IsDeletedSuccess]
                       ,[MigratedFilterWhere]
                       ,[DeletedFilterWhere]
                       ,[BeginTime]
                       ,[Duration])
                 VALUES
                       (@TableName
                       ,@TargetDbName
                       ,@SourceDataCount
                       ,@MigratedCount
                       ,@DateOfMigratedData
                       ,@IsMigratedSuccess
                       ,@DeletedSourceCount
                       ,@DateOfDeletedData
                       ,@IsDeletedSuccess
                       ,@MigratedFilterWhere
                       ,@DeletedFilterWhere
                       ,@BeginTime
                       ,@Duration);
                        ";
            using (var connection = new SqlConnection(ConnectionStrings.Exam8_SystemconnStr))
            {
                connection.Execute(sql, new
                {
                    TableName = _SourceTableName,
                    TargetDBName = _TargetDbName,
                    SourceDataCount = _SourceDataCount,
                    MigratedCount = _MigratedCount,
                    DateOfMigratedData = _DateOfMigratedData,
                    IsMigratedSuccess = _IsMigratedSuccess,
                    DeletedSourceCount = _DeletedSourceCount,
                    DateOfDeletedData = _DateOfDeletedData,
                    IsDeletedSuccess = _IsDeletedSuccess,
                    MigratedFilterWhere = _MigratedFilterWhere,
                    DeletedFilterWhere = _DeletedFilterWhere,
                    BeginTime = _BeginTime,
                    Duration = DateTimeHelper.TotalSpaceTime(_BeginTime, DateTime.Now, Utils.UtilEnum.TimeSpaceType.Milliseconds),
                });
            }
        }
    }
}
