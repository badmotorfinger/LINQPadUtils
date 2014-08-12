namespace LINQPadUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using LINQPad;
    using LINQPad.Extensibility.DataContext.DbSchema;

    public class TableBuilder
    {
        readonly IEnumerable<object> collection;

        readonly TableHeading[] headings;

        public TableBuilder(IEnumerable<object> collection)
        {
            this.collection = collection;
            Type objType = collection.GetType().GetGenericArguments().First();

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
                    rowDataSb.AppendFormat("<td>{0}</td>", this.GetPropertyValue(
                        tableHeading.PropertyInfo, item));
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
            var mi =
                Assembly.GetAssembly(typeof(ExecutionEngine))
                        .GetTypes()
                        .First(a => a.Name.Contains("Raw"))
                        .GetFields()
                        .First(a => a.Name.Contains("Html"));

            var value = mi.GetValue(obj);

            if (pi.PropertyType == mi.FieldType)
            {
                return mi.GetValue(obj).ToString();
            }

            return obj.ToString();
        }

        public override string ToString()
        {
            var renderedHeadings = this.GetHeadingsAsHtml();
            var renderedRows = this.GetRowDataAsHtml();

            var resultBuilder = new StringBuilder(LinqPadUtils.ResultTable);

            resultBuilder
                .Replace("{colspan}", this.headings.Length.ToString())
                .Replace("{typename}", collection.GetType().ToString())
                .Replace("{itemcount}", collection.Count().ToString(CultureInfo.InvariantCulture))
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