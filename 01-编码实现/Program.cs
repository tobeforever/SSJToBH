using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSJToYY.Helper;
using SSJToYY.Model;

namespace SSJToYY
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            #region 描述信息

            Console.WriteLine("本工具可将随手记导出csv格式文件转换为一羽记账格式csv文件，支持“收入”、“支出”、“转入”、“转出”四种类型记录。可导出账单记录及收支分类文件，两文件在手机端可直接打开导入到一羽记账。注意：导入收支分类时无法覆盖现有的收支分类，如有需要，请先清空当前APP端收支分类再进行导入。");
            Console.WriteLine("当前版本：V20210930");
            Console.WriteLine("开源地址：https://github.com/tobeforever/SSJToYY");

            #endregion

            try
            {
                #region 参数操作

                Console.WriteLine("请输入待处理文件完整路径：");

                string ssjFilePath;
                while (true)
                {
                    ssjFilePath = Console.ReadLine();
                    // 判断待处理文件是否存在
                    if (!File.Exists(ssjFilePath))
                    {
                        Console.WriteLine("待处理文件不存在,请重新输入！");
                    }
                    break;
                }

                if (string.IsNullOrEmpty(ssjFilePath))
                {
                    throw new Exception("待处理文件路径无法识别！");
                }
                var ssjFile = new FileInfo(ssjFilePath);

                #endregion

                #region  随手记数据处理

                // 随手记数据
                List<SsjData> ssjDataList = CsvFileHelper.ReadSsjDataFromFile(ssjFile.FullName, out string unidentificationData);

                Console.WriteLine("解析文件成功！");
                // 非法数据
                List<SsjData> illegalityData = ssjDataList.Where(data =>
                    {
                        return (data.DealType != "支出" &&
                                data.DealType != "收入" &&
                                data.DealType != "转账" ) ||
                               (data.DealType == "转账" && string.IsNullOrEmpty(data.Account2)
                               );
                    }
                ).ToList();

                // 输出未识别数据

                string errFileName = ssjFile.DirectoryName + "\\未识别随手记数据-" + ssjFile.Name;
                CsvFileHelper.SaveSsjDataToFile(illegalityData, unidentificationData, errFileName);
                Console.WriteLine($"未识别随手记数据输出路径：{errFileName}");

                // 移除非法数据
                foreach (var errData in illegalityData)
                {
                    ssjDataList.Remove(errData);
                }

                // 输出合法数据记录数
                Console.WriteLine($"合法支出数据条数：{ssjDataList.Count(data => data.DealType == "支出")}");
                Console.WriteLine($"合法收入数据条数：{ssjDataList.Count(data => data.DealType == "收入")}");
                Console.WriteLine($"合法转账数据条数：{ssjDataList.Count(data => data.DealType == "转账")}");

                #endregion

                #region 一羽数据处理

                // 一羽数据
                List<YyjzData> bhjzDataList = new List<YyjzData>();

                // 随手记数据处理为一羽记账数据
                foreach (SsjData ssjData in ssjDataList)
                {
                    bhjzDataList.Add(new YyjzData(ssjData));

                }

                // 按时间排序
                bhjzDataList.Sort((x, y) => x.DataTime >= y.DataTime ? -1 : 1);

                // 输出数据数量
                Console.WriteLine($"转换后数据数量：{bhjzDataList.Count}");
                if (bhjzDataList.Count > 0)
                {
                    string dataFileName = ssjFile.DirectoryName + "\\一羽记账格式数据-" + ssjFile.Name;
                    CsvFileHelper.SaveYyjzDataToFile(bhjzDataList, dataFileName);
                    Console.WriteLine($"转换后数据输出路径：{dataFileName}");
                }

                #endregion

                #region 一羽收支分类处理

                // 一羽收支分类数据
                List<YyjzClassification> yyjzClassificationDataList = new List<YyjzClassification>();

                // 随手记数据中提取分类关系
                foreach (SsjData ssjData in ssjDataList)
                {
                    try
                    {
                        // 转账类型不提取分类关系
                        if (ssjData.DealType is "转账")
                            continue;

                        // 识别父类
                        YyjzClassification parentType = yyjzClassificationDataList.FirstOrDefault(type => type.name == ssjData.RecordType);
                        if (parentType == null)
                        {
                            parentType = new YyjzClassification
                            {
                                name = ssjData.RecordType,
                                type = ssjData.DealType == "收入" ? 1 : 0
                            };
                            yyjzClassificationDataList.Add(parentType);
                        }

                        // 识别子类
                        if (!string.IsNullOrEmpty(ssjData.SubRecordType))
                        {
                            parentType.child ??= new List<YyjzClassification>();

                            YyjzClassification subType = parentType.child.FirstOrDefault(type => type.name == ssjData.SubRecordType);
                            if (subType == null)
                            {
                                subType = new YyjzClassification
                                {
                                    name = ssjData.SubRecordType,
                                    type = parentType.type
                                };
                                parentType.child.Add(subType);
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                if (bhjzDataList.Count > 0)
                {
                    string typeFileName = ssjFile.DirectoryName + "\\一羽记账收支分类-" +
                                         DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    JsonFileHelper.SaveYyjzTypeToFile(yyjzClassificationDataList, typeFileName);
                    Console.WriteLine($"收支分类输出路径：{typeFileName}");
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.Write($"转换出错：{ex.Message}");
            }

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
}
