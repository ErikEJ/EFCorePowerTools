using System;
using System.IO;
using System.Text;
using System.Linq;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class LiteralParser : Parser
    {
        public override Token Parse(TextReader reader)
        {
            LiteralExpressionToken token = null;
            if (IsDelimitedLiteral(reader) == true)
            {
                char delimiter = (char)reader.Read();
                var buffer = AccumulateLiteral(reader, true, delimiter);
                token = Token.Literal(buffer, delimiter.ToString());
            }
            else if (IsNonDelimitedLiteral(reader) == true)
            {
                var buffer = AccumulateLiteral(reader, false, ' ', ')');
                token = Token.Literal(buffer);
            }
            return token;
        }

        private static bool IsDelimitedLiteral(TextReader reader)
        {
            var peek = (char)reader.Peek();
            return peek == '\'' || peek == '"';
        }

        private static bool IsNonDelimitedLiteral(TextReader reader)
        {
            var peek = (char)reader.Peek();
            return char.IsDigit(peek) || peek == '-';
        }

        private static string AccumulateLiteral(TextReader reader, bool captureDelimiter, params char[] delimiters)
        {
            StringBuilder buffer = new StringBuilder();
            while (true)
            {
                var node = reader.Peek();
                if (node == -1)
                {
                    throw new InvalidOperationException("Reached end of template before the expression was closed.");
                }
                else
                {
                    if (delimiters.Contains((char)node))
                    {
                        if (captureDelimiter)
                        {
                            reader.Read();
                        }
                        break;
                    }
                    else if (!captureDelimiter && (char)node == '}')
                    {
                        break;
                    }
                    else
                    {
                        buffer.Append((char)reader.Read());
                    }
                }
            }
            return buffer.ToString();
        }
    }
}

