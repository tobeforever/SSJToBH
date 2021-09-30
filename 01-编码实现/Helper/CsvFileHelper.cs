using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SSJToYY.Model;

namespace SSJToYY.Helper
{
    public static class CsvFileHelper
    {
        #region 随手记

        /// <summary>
        /// 从随手记文件中读取数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="unidentificationData"></param>
        /// <returns></returns>
        public static List<SsjData> ReadSsjDataFromFile(string filePath, out string unidentificationData)
        {
            // 判断待处理文件是否存在
            if (!File.Exists(filePath))
            {
                throw new Exception("待处理文件不存在！");
            }

            StringBuilder sb = new StringBuilder();
            List<SsjData> ssjDataList = new List<SsjData>();

            using (StreamReader fileReader = new StreamReader(filePath))
            {
                string strLine = "";
                while (strLine != null)
                {
                    // 读取一行
                    strLine = fileReader.ReadLine();
                    if (!string.IsNullOrEmpty(strLine))
                    {
                        // 解析此行数据
                        string[] recodeData = strLine.Replace("\"", "").Split(',');

                        try
                        {
                            // 跳过起始描述行
                            if (recodeData[0].StartsWith("随手记导出文件") || recodeData[0].StartsWith("交易类型"))
                            {
                                throw new Exception();
                            }
                            ssjDataList.Add(new SsjData(recodeData));
                        }
                        // 记录未识别数据
                        catch
                        {
                            sb.AppendLine(strLine);
                        }
                    }
                }
            }

            unidentificationData = sb.ToString();
            return ssjDataList;
        }

        /// <summary>
        /// 保存随手记数据到文件
        /// </summary>
        /// <param name="ssjDataList"></param>
        /// <param name="unidentificationData"></param>
        /// <param name="filePath"></param>
        public static void SaveSsjDataToFile(List<SsjData> ssjDataList, string unidentificationData, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StreamWriter fileWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            // 未识别数据
            fileWriter.WriteLine(unidentificationData);
            // 数据
            foreach (SsjData data in ssjDataList)
            {
                fileWriter.WriteLine(data.ToString());
            }
            fileWriter.Flush();
            fileWriter.Close();
        }

        #endregion

        #region 一羽

        /// <summary>
        /// 保存一羽记账数据到文件
        /// </summary>
        /// <param name="bhjzDataList"></param>
        /// <param name="filePath"></param>
        public static void SaveYyjzDataToFile(List<YyjzData> bhjzDataList, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StreamWriter fileWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            // 标题
            fileWriter.WriteLine(YyjzData.ToTitleString());
            // 数据
            foreach (var data in bhjzDataList)
            {
                fileWriter.WriteLine(data.ToString());
            }
            fileWriter.Flush();
            fileWriter.Close();
        }

        #endregion
    }
}
