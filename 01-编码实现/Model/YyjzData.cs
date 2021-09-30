using System;

namespace SSJToYY.Model
{
    /// <summary>
    /// 一羽记账数据对象
    /// </summary>
    public class YyjzData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DataTime = DateTime.MinValue;

        /// <summary>
        /// 类型
        /// </summary>
        public string DealType = string.Empty;

        /// <summary>
        /// 分类
        /// </summary>
        public string SubRecordType = string.Empty;

        /// <summary>
        /// 金额
        /// </summary>
        public double Money = 0;

        /// <summary>
        /// 账户1(付款账户)
        /// </summary>
        public string PayAccount = string.Empty;

        /// <summary>
        /// 账户2(收款账户)
        /// </summary>
        public string ReceiveAccount = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark = string.Empty;

        /// <summary>
        /// 关联ID
        /// </summary>
        public string RelevanceID = string.Empty;

        public YyjzData(SsjData ssjData)
        {
            if (ssjData.DealType == "转出")
            {
                DealType = "转账";
                SubRecordType = "账户间转账";
                PayAccount = ssjData.Account;
            }
            else if (ssjData.DealType == "转入")
            {
                DealType = "转账";
                SubRecordType = "账户间转账";
                ReceiveAccount = ssjData.Account;
            }
            else
            {
                DealType = ssjData.DealType;
                SubRecordType = ssjData.SubRecordType;
                PayAccount = ssjData.Account;
            }

            Money = ssjData.Money;
            DataTime = ssjData.DataTime;
            Remark = ssjData.Remark;
            RelevanceID = ssjData.RelevanceID;
        }

        public override string ToString()
        {
            return $"{DataTime:yyyy/MM/dd HH:mm},{DealType},{SubRecordType},{Money:f2},{PayAccount},{ReceiveAccount},{Remark}";
        }

        public static string ToTitleString()
        {
            return "日期,类型,分类,金额,账户,账户2,备注";
        }
    }
}
