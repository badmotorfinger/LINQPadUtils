namespace LINQPadUtils
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using LINQPad;

    public class TableBuilder
    {
        readonly IEnumerable collection;

        readonly TableHeading[] headings;

        public TableBuilder(object dumpObj)
        {
            if (dumpObj is IEnumerable)
            {
                this.collection = (IEnumerable)dumpObj;
            }
            else
            {
                this.collection = new[] { dumpObj };
            }

            var objectType = dumpObj.GetType();

            Type objType =
                objectType.IsGenericType
                    ? dumpObj.GetType().GetGenericArguments().First()
                    : objectType;

            this.headings =
                objType.GetProperties()
                   .Select(p => new TableHeading
                   {
                       Title = p.Name,
                       Type = p.PropertyType,
                       PropertyInfo = p
                   })
                   .ToArray();
        }

        StringBuilder GetRowDataAsHtml()
        {
            var rowDataSb = new StringBuilder();

            foreach (var item in collection)
            {
                rowDataSb.Append("<tr>");

                foreach (var tableHeading in headings)
                {
                    rowDataSb.AppendFormat("<td>{0}</td>",

                        this.GetPropertyValue(tableHeading.PropertyInfo, item));
                }

                rowDataSb.Append("</tr>");
            }

            return rowDataSb;
        }

        StringBuilder GetHeadingsAsHtml()
        {
            var headingsSb = new StringBuilder("<tr>");

            foreach (var tableHeading in headings)
            {
                var heading = String.Format("<th title='{0}'>{1}</th>", tableHeading.Type, tableHeading.Title);

                headingsSb.Append(heading);
            }

            return headingsSb.Append("</tr>");
        }

        string GetPropertyValue(PropertyInfo pi, object obj)
        {
            var objectType = obj.GetType();

            if (objectType.IsPrimitive
                            ||
                            objectType == typeof(string)) return obj.ToString();

            var property = pi.GetValue(obj);

            if (property == null)
                return null;

            var propertyType = property.GetType();

            if (propertyType == typeof(string)) return property.ToString();

            var mi =
                Assembly.GetAssembly(typeof(ExecutionEngine))
                        .GetType("LINQPad.ObjectGraph.RawHtml");

            if (propertyType == mi)
            {
                var fi = mi.GetField("Html");

                return fi.GetValue(property).ToString();
            }

            return new TableBuilder(property).ToString();
        }

        public override string ToString()
        {
            var renderedHeadings = this.GetHeadingsAsHtml();
            var renderedRows = this.GetRowDataAsHtml();

            var resultBuilder = new StringBuilder(LinqPadUtils.ResultTable);

            int count = collection.Cast<object>().Count();

            resultBuilder
                .Replace("{colspan}", this.headings.Length.ToString())
                .Replace("{typename}", collection.GetType().ToString())
                .Replace("{itemcount}", count.ToString(CultureInfo.InvariantCulture))
                .Replace("{headings}", renderedHeadings.ToString())
                .Replace("{rows}", renderedRows.ToString());

            return LinqPadUtils.DumpExtended.Replace("{content}", resultBuilder.ToString());
        }
    }

    public class TableHeading
    {
        public string Title { get; set; }

        public Type Type { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

    }
}