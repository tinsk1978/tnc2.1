using System;
using System.Data;
using System.IO;
using System.Text;

namespace tsl.File {
    /// <summary>
    /// csv文件操作类
    /// </summary>
    public class Csv {
        /* 
         * csv文件格式说明：
         * 以半角逗号（即,）作分隔符，列为空也要表达其存在。
         * 列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
         * 列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
         * 
         */

        /// <summary>
        /// 将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns></returns>
        public static string DataTableToCsv(DataTable table) {
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            int count = table.Columns.Count;
            foreach(DataRow row in table.Rows) {
                for(int i = 0;i<count;i++) {
                    colum=table.Columns[i];
                    if(i!=0) {
                        _=sb.Append(",");
                    }
                    if(colum.DataType==typeof(string)&&row[colum].ToString().Contains(",")) {
                        _=sb.Append("\""+row[colum].ToString().Replace("\"","\"\"")+"\"");
                    } else {
                        _=sb.Append(row[colum].ToString());
                    }
                }
                _=sb.AppendLine();
            }
            return sb.ToString();
        }
        /// <summary>
        /// 将DataTable转换为标准的CSV文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="CsvFileName"></param>
        public static void DataTableToCsvFile(DataTable table,string CsvFileName) {
            string csv = DataTableToCsv(table);
            System.IO.File.WriteAllText(CsvFileName,csv);
        }
    }
}
