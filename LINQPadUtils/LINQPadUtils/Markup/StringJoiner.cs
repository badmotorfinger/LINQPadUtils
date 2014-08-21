namespace LINQPadUtils.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StringJoiner
    {
        readonly internal StringBuilder Document;

        readonly string footer;

        readonly List<Func<string>> renderFuncs = new List<Func<string>>();

        public StringJoiner()
        {
            this.Document = new StringBuilder();
            this.footer = String.Empty;
        }

        public StringJoiner(string head, string foot)
        {
            this.Document = new StringBuilder(head);
            this.footer = foot;
        }

        public void AppendFunc(Func<string> renderFunc)
        {
            this.renderFuncs.Add(renderFunc);
        }

        public void Append(string s)
        {
            this.Document.Append(s);
        }

        public override string ToString()
        {
            foreach (var renderFunc in this.renderFuncs)
            {
                this.Document.Append(renderFunc());
            }

            return this.Document.Append(this.footer)
                .ToString();
        }
    }
}