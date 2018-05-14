using System;
using System.Globalization;
using System.Text;

namespace HandlebarsDotNet
{
    public class HtmlEncoder : ITextEncoder
    {
        public string Encode(string text)
        {
            if (text == null)
                return String.Empty;

            var sb = new StringBuilder(text.Length);

            for (var i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '"':
                        sb.Append("&quot;");
                        break;
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;

                    default:
                        if (text[i] > 159)
                        {
                            sb.Append("&#");
                            sb.Append(((int) text[i]).ToString(CultureInfo.InvariantCulture));
                            sb.Append(";");
                        }
                        else
                            sb.Append(text[i]);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}