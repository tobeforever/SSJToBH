using System.Collections.Generic;
using System.IO;
using System.Text;
using SSJToYY.Model;

namespace SSJToYY.Helper
{
    public static class JsonFileHelper
    {
        /// <summary>
        /// 保存一羽记账收支分类到文件
        /// </summary>
        /// <param name="yyjzTypeList"></param>
        /// <param name="filePath"></param>
        public static void SaveYyjzTypeToFile(List<YyjzClassification> yyjzTypeList, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StreamWriter fileWriter = new StreamWriter(filePath, false, Encoding.UTF8);

            fileWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(yyjzTypeList));

            fileWriter.Flush();
            fileWriter.Close();
        }
    }
}
