using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class Tokenizer
    {
        private readonly HandlebarsConfiguration _configuration;

        private static Parser _wordParser = new WordParser();
        private static Parser _literalParser = new LiteralParser();
        private static Parser _commentParser = new CommentParser();
        private static Parser _partialParser = new PartialParser();
        private static Parser _blockWordParser = new BlockWordParser();

        public Tokenizer(HandlebarsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Token> Tokenize(TextReader source)
        {
            try
            {
                return Parse(source);
            }
            catch (Exception ex)
            {
                throw new HandlebarsParserException("An unhandled exception occurred while trying to compile the template", ex);
            }
        }

        private IEnumerable<Token> Parse(TextReader source)
        {
            bool inExpression = false;
            bool trimWhitespace = false;
            var buffer = new StringBuilder();
            var node = source.Read();
            while (true)
            {
                if (node == -1)
                {
                    if (buffer.Length > 0)
                    {
                        if (inExpression)
                        {
                            throw new InvalidOperationException("Reached end of template before expression was closed");
                        }
                        else
                        {
                            yield return Token.Static(buffer.ToString());
                        }
                    }
                    break;
                }
                if (inExpression)
                {
                    if ((char)node == '(')
                    {
                        yield return Token.StartSubExpression();
                    }

                    Token token = null;
                    token = token ?? _wordParser.Parse(source);
                    token = token ?? _literalParser.Parse(source);
                    token = token ?? _commentParser.Parse(source);
                    token = token ?? _partialParser.Parse(source);
                    token = token ?? _blockWordParser.Parse(source);

                    if (token != null)
                    {
                        yield return token;
                    }
                    if ((char)node == '}' && (char)source.Read() == '}')
                    {
                        bool escaped = true;
                        if ((char)source.Peek() == '}')
                        {
                            node = source.Read();
                            escaped = false;
                        }
                        node = source.Read();
                        yield return Token.EndExpression(escaped, trimWhitespace);
                        inExpression = false;
                    }
                    else if ((char)node == ')')
                    {
                        node = source.Read();
                        yield return Token.EndSubExpression();
                    }
                    else if (char.IsWhiteSpace((char)node) || char.IsWhiteSpace((char)source.Peek()))
                    {
                        node = source.Read();
                    }
                    else if ((char)node == '~')
                    {
                        node = source.Read();
                        trimWhitespace = true;
                    }
                    else
                    {
                        if (token == null)
                        {
                            
                            throw new HandlebarsParserException("Reached unparseable token in expression: " + source.ReadLine());
                        }
                        node = source.Read();
                    }
                }
                else
                {
                    if ((char)node == '\\' && (char)source.Peek() == '{')
                    {
                        source.Read();
                        if ((char)source.Peek() == '{')
                        {
                            source.Read();
                            buffer.Append('{', 2);
                        }
                        else
                        {
                            buffer.Append("\\{");
                        }
                        node = source.Read();
                    }
                    else if ((char)node == '{' && (char)source.Peek() == '{')
                    {
                        bool escaped = true;
                        trimWhitespace = false;
                        node = source.Read();
                        if ((char)source.Peek() == '{')
                        {
                            node = source.Read();
                            escaped = false;
                        }
                        if ((char)source.Peek() == '~')
                        {
                            source.Read();
                            node = source.Peek();
                            trimWhitespace = true;
                        }
                        yield return Token.Static(buffer.ToString());
                        yield return Token.StartExpression(escaped, trimWhitespace);
                        trimWhitespace = false;
                        buffer = new StringBuilder();
                        inExpression = true;
                    }
                    else
                    {
                        buffer.Append((char)node);
                        node = source.Read();
                    }
                }
            }
        }
    }
}

