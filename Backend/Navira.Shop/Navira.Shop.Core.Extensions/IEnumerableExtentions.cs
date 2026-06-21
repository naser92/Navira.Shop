using System.Data;

namespace Navira.Shop.Core.Extensions
{
    public static class IEnumerableExtentions
    {
        public static DataTable ToDataTable(this IEnumerable<object> v)
        {
            //We really want to know if there is any data at all
            var firstRecord = v.FirstOrDefault();
            if (firstRecord.IsNull())
                return null;


            // We have data and we can work on it
            //So what do you have?
            var props = firstRecord.GetType().GetProperties();

            //Our table should have the columns to support the properties
            DataTable table = new DataTable();

            //Add, add, add the columns
            foreach (var prop in props)
            {

                Type propType = prop.PropertyType;

                if ((propType.IsGenericType
                    && propType.GetGenericTypeDefinition() == typeof(Nullable<>)) || !propType.IsValueType) //Nullable types should be handled too
                    propType = Nullable.GetUnderlyingType(propType) ?? propType;

                table.Columns.Add(prop.Name, propType);
            }

            foreach (var record in v)
            {
                var row = table.NewRow();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(record);
                    if (value is string && ((string)value).IsNullOrWhiteSpace())
                        value = null;
                    row[prop.Name] = value ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            //Table is ready to serve.
            table.AcceptChanges();

            return table;
        }
        public static DataTable ToDataTableOfString(this IEnumerable<object> v)
        {
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            var infos = firstRecord.GetType().GetProperties();

            DataTable table = new DataTable();
            foreach (var info in infos)
            {
                table.Columns.Add(info.Name, typeof(string));
            }

            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = (infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value).ToString();
                }
                table.Rows.Add(row);
            }

            table.AcceptChanges();

            return table;
        }
        public static string[] ToCacheKey(this IEnumerable<string> v)
        {
            if (v == null) return null;

            List<string> result = new List<string>();

            var key = v.First();

            foreach (var record in v.Skip(1))
            {
                key += "." + record;
                result.Add(key);
            }
            return result.ToArray();
        }
        public static string Join(this IEnumerable<string> v, string seperator)
        {
            return StringExtension.Join(v.ToArray(), seperator);
        }
    }
}
