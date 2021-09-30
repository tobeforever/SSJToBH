using System.Collections.Generic;
using Newtonsoft.Json;

namespace SSJToYY.Model
{
    /// <summary>
    /// 一羽记账收支分类
    /// </summary>
    public class YyjzClassification
    {
        /// <summary>
        /// 子类
        /// </summary>
        [JsonProperty(PropertyName = "child", NullValueHandling = NullValueHandling.Ignore)]
        public List<YyjzClassification> child;

        /// <summary>
        /// 分类名称
        /// </summary>
        public string name;

        /// <summary>
        /// 收支类型
        /// 0支出 1收入
        /// </summary>
        public int type;

        /// <summary>
        /// 项目图标
        /// </summary>
        public string icon = "record_cat_unknown";
    }
}
