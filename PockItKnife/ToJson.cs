using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PockItKnife
{
    public class ToJson
    {
        private readonly Type[] __NUMERIC_TYPES = new[] { typeof(double), typeof(int), typeof(short), typeof(float), typeof(decimal), typeof(long), typeof(ulong), typeof(uint), typeof(ushort) };
        private const string __DELIMITER = "'";

        public ToJson()
        {
        }

        private string Convert(string key, object value, Type type)
        {
            if (key.IsNullOrEmpty())
                return "";

            return "{2}{0}{2}: {1}".Inject(key, ConvertValue(value, type), __DELIMITER);
        }

        private object ConvertValue(object value, Type type)
        {
            if (value == null)
                value = "null";
            else {
                string delimiter = "";
                if (type == typeof(bool))
                    value = value.ToString().ToLower();
                else if (__NUMERIC_TYPES.Contains(type))
                    value = value.ToString().Replace(",", ".").ToLower();
                else {
                    delimiter = __DELIMITER;
                    value = value.ToString().Replace(delimiter, @"\" + delimiter);
                }

                value = delimiter + value + delimiter;
            }
            return value;
        }

        public string ConvertDataTable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return "[]";

            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            string delimitRow = "";
            foreach (DataRow dr in dt.Rows) {
                builder.AppendFormat("{1}{0}", ConvertDataRow(dr), delimitRow);
                delimitRow = ",";
            }
            builder.Append("]");

            return builder.ToString();
        }

        public string ConvertDataRow(DataRow dr)
        {
            string delimitColumn = "";
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (DataColumn dc in dr.Table.Columns) {
                builder.AppendFormat("{1}{0}", Convert(dc.ColumnName, dr[dc], dc.DataType), delimitColumn);
                delimitColumn = ",";
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
