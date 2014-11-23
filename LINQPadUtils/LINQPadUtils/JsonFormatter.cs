namespace LINQPad
{
    using System;
    using System.Linq;

    public static class JsonFormatter
    {
        const string IndentString = "  ";

        public static string FormatJson(string json)
        {
            int indentation = 0;
            int quoteCount = 0;

            var result =
                from ch in json
                let quotes = (ch == '"' || ch == '\'')
                    ? quoteCount++
                    : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0
                    ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(IndentString, indentation))
                    : null
                let openChar = ch == '{' || ch == '['
                    ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(IndentString, ++indentation))
                    : ch.ToString()
                let closeChar = ch == '}' || ch == ']'
                    ? Environment.NewLine + String.Concat(Enumerable.Repeat(IndentString, --indentation)) + ch
                    : ch.ToString()

                // When quotes are closed and there's a space, ignore it unless we are between quotes.
                where (quotes % 2 == 0 && !Char.IsSeparator(ch)) || quotes % 2 == 1
                select lineBreak ??
                       (openChar != null && openChar.Length > 1
                           ? openChar
                           : closeChar);

            return String.Concat(result);
        }
    }
}