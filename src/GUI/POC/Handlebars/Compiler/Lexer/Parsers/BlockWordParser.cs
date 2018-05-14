using System;
using System.IO;
using System.Linq;
using System.Text;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class BlockWordParser : Parser
    {
        private const string validBlockWordStartCharacters = "#^/";

        public override Token Parse(TextReader reader)
        {
            WordExpressionToken token = null;
            if (IsBlockWord(reader))
            {
                var buffer = AccumulateBlockWord(reader);
                token = Token.Word(buffer);
            }
            return token;
        }

        private bool IsBlockWord(TextReader reader)
        {
            var peek = (char)reader.Peek();
            return validBlockWordStartCharacters.Contains(peek.ToString());
        }

        private string AccumulateBlockWord(TextReader reader)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append((char)reader.Read());
            while(char.IsWhiteSpace((char)reader.Peek()))
            {
                reader.Read();
            }
            while(true)
            {
                var peek = (char)reader.Peek();
                if (peek == '}' || peek == '~' || char.IsWhiteSpace(peek))
                {
                    break;
                }
                var node = reader.Read();
                if (node == -1)
                {
                    throw new InvalidOperationException("Reached end of template before the expression was closed.");
                }
                else
                {
                    buffer.Append((char)node);
                }
            }
            return buffer.ToString();
        }
    }
}

