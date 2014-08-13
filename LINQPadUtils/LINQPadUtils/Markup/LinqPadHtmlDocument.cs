using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQPadUtils.Markup
{
    using System.Runtime.InteropServices.WindowsRuntime;

    public class LinqPadHtmlDocument
    {
        readonly StringBuilder document;

        readonly string footer;

        readonly List<Func<string>> renderFuncs = new List<Func<string>>();

        public LinqPadHtmlDocument()
        {
            this.document = new StringBuilder(LinqPadUtilResources.DumpExtendedHead);
            this.footer = LinqPadUtilResources.DumpExtendedFoot;
        }
        public LinqPadHtmlDocument(string head, string foot)
        {
            this.document = new StringBuilder(head);
            this.footer = foot;
        }

        public void AddRenderer(Func<string> renderFunc)
        {
            renderFuncs.Add(renderFunc);
        }

        public override string ToString()
        {
            foreach (var renderFunc in renderFuncs)
            {
                document.Append(renderFunc());
            }

            return document.Append(LinqPadUtilResources.DumpExtendedFoot)
                           .ToString();
        }
    }
}
