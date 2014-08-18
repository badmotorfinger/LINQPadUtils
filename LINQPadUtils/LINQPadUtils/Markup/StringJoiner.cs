using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQPadUtils.Markup
{
    using System.Runtime.InteropServices.WindowsRuntime;

    public class StringJoiner
    {
        internal StringBuilder document;

        readonly string footer;

        readonly List<Func<string>> renderFuncs = new List<Func<string>>();

        public StringJoiner()
        {
            this.document = new StringBuilder();
            this.footer = String.Empty;
        }

        public StringJoiner(string head, string foot)
        {
            this.document = new StringBuilder(head);
            this.footer = foot;
        }

        public void AppendFunc(Func<string> renderFunc)
        {
            renderFuncs.Add(renderFunc);
        }

        public void Append(string s)
        {
            document.Append(s);
        }

        public override string ToString()
        {
            foreach (var renderFunc in renderFuncs)
            {
                document.Append(renderFunc());
            }

            return document.Append(footer)
                           .ToString();
        }
    }
}