using System;

namespace SSJToYY.Model
{
    /// <summary>
    /// 随手记数据对象
    /// </summary>
    public class SsjData
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public string DealType = string.Empty;

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime DataTime = DateTime.MinValue;

        /// <summary>
        /// 记录类型
        /// </summary>
        public string RecordType = string.Empty;

        /// <summary>
        /// 记录子类型
        /// </summary>
        public string SubRecordType = string.Empty;

        /// <summary>
        /// 项目
        /// </summary>
        public string Project = string.Empty;

        /// <summary>
        /// 账户
        /// </summary>
        public string Account = string.Empty;

        /// <summary>
        /// 货币种类
        /// </summary>
        public string CurrencyType = string.Empty;

        /// <summary>
        /// 金额
        /// </summary>
        public double Money = 0;

        /// <summary>
        /// 成员
        /// </summary>
        public string Member = string.Empty;

        /// <summary>
        /// 商家
        /// </summary>
        public string Merchant = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark = string.Empty;

        /// <summary>
        /// 关联ID
        /// </summary>
        public string RelevanceID = string.Empty;

        public SsjData(string[] recodeData)
        {
            if (recodeData.Length == 12)
            {
                DealType = recodeData[0];
                DateTime.TryParse(recodeData[1], out DataTime);
                RecordType = recodeData[2];
                SubRecordType = recodeData[3];
                Project = recodeData[4];
                Account = recodeData[5];
                CurrencyType = recodeData[6];
                double.TryParse(recodeData[7], out Money);
                Member = recodeData[8];
                Merchant = recodeData[9];
                Remark = recodeData[10];
                RelevanceID = recodeData[11];
            }
            else
            {
                throw new Exception("输入数据非法");
            }
        }

        public override string ToString()
        {
            return $"{DealType},{DataTime:yyyy-MM-dd HH:mm:ss},{RecordType},{SubRecordType},{Project},{Account},{CurrencyType},{Money:f2},{Member},{Merchant},{Remark},{RelevanceID}";
        }

        public static string ToTitleString()
        {
            return "交易类型,日期,类别,子类别,项目,账户,账户币种,金额,成员,商家,备注,关联Id";
        }
    }
}
