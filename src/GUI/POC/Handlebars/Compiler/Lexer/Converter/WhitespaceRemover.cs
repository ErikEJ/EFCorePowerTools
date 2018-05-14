using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Compiler.Lexer;
using System.Text.RegularExpressions;

namespace HandlebarsDotNet.Compiler
{
    internal class WhitespaceRemover : TokenConverter
    {
        private static readonly Regex MatchLastStartsWithWhitespace = new Regex(@"^[ \t]*(\r?\n|$)");
        private static readonly Regex MatchStartsWithWhitespace = new Regex(@"^[ \t]*\r?\n");
        private static readonly Regex TrimStartRegex = new Regex(@"^[ \t]*\r?\n?");
        private static readonly Regex MatchFirstEndsWithWhitespace = new Regex(@"(^|\r?\n)\s*?$");
        private static readonly Regex MatchEndsWithWhitespace = new Regex(@"\r?\n\s*?$");
        private static readonly Regex TrimEndRegex = new Regex(@"[ \t]+\z");

        public static IEnumerable<object> Remove(IEnumerable<object> sequence)
        {
            return new WhitespaceRemover().ConvertTokens(sequence);
        }

        private WhitespaceRemover()
        {
        }

        private static List<object> ToList(IEnumerable<object> sequence)
        {
            //it's already List<object> but let's pretend we don't know.
            return sequence as List<object> ?? sequence.ToList();
        }

        public override IEnumerable<object> ConvertTokens(IEnumerable<object> sequence)
        {
            var list = ToList(sequence);

            ProcessTokens(list);

            return list;
        }

        private static void ProcessTokens(IList<object> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var statement = list[i] as StatementExpression;
                if (statement == null) continue;

                if (statement.TrimBefore)
                {
                    TrimBefore(list, i, true);
                }
                if (statement.TrimAfter)
                {
                    TrimAfter(list, i, true);
                }

                if (IsStandalone(statement) && IsNextWhitespace(list, i) && IsPrevWhitespace(list, i))
                {
                    if (!statement.TrimBefore)
                    {
                        TrimBefore(list, i, false);
                    }
                    if (!statement.TrimAfter)
                    {
                        TrimAfter(list, i, false);
                    }
                }
            }
        }

        private static bool IsNextWhitespace(IList<object> list, int index)
        {
            if (index >= list.Count - 1)
            {
                return true;
            }

            var next = list[index + 1] as StaticToken;

            if (next == null)
            {
                return false;
            }

            var nextIsLast = index == list.Count - 2;

            var regex = nextIsLast ? MatchLastStartsWithWhitespace : MatchStartsWithWhitespace;

            return regex.IsMatch(next.Original);
        }

        private static void TrimAfter(IList<object> list, int index, bool multipleLines)
        {
            if (index >= list.Count - 1)
            {
                return;
            }

            var next = list[index + 1] as StaticToken;

            if (next == null)
            {
                return;
            }

            list[index + 1] = TrimStart(next, multipleLines);
        }

        private static Token TrimStart(StaticToken token, bool multipleLines)
        {
            var value = multipleLines
                ? token.Value.TrimStart()
                : TrimStartRegex.Replace(token.Value, String.Empty);

            return token.GetModifiedToken(value);
        }

        private static bool IsPrevWhitespace(IList<object> list, int index)
        {
            if (index < 1)
            {
                return true;
            }

            var prev = list[index - 1] as StaticToken;

            if (prev == null)
            {
                return false;
            }

            var prevIsFirst = index == 1;

            var regex = prevIsFirst ? MatchFirstEndsWithWhitespace : MatchEndsWithWhitespace;

            return regex.IsMatch(prev.Original);
        }

        private static void TrimBefore(IList<object> list, int index, bool multipleLines)
        {
            if (index < 1)
            {
                return;
            }

            var prev = list[index - 1] as StaticToken;

            if (prev == null)
            {
                return;
            }

            list[index - 1] = TrimEnd(prev, multipleLines);
        }

        private static Token TrimEnd(StaticToken token, bool multipleLines)
        {
            var value = multipleLines
                ? token.Value.TrimEnd()
                : TrimEndRegex.Replace(token.Value, String.Empty);

            return token.GetModifiedToken(value);
        }

        private static bool IsStandalone(StatementExpression statement)
        {
            return statement.Body is CommentExpression ||
                   statement.Body is PartialExpression ||
                   IsBlockStatement(statement);
        }

        private static bool IsBlockStatement(StatementExpression statement)
        {
            return IsBlockHelperOrInversion(statement.Body as HelperExpression) ||
                   IsSectionOrClosingNode(statement.Body as PathExpression);
        }

        private static bool IsSectionOrClosingNode(PathExpression pathExpression)
        {
            return (pathExpression != null) && pathExpression.Path.IndexOfAny(new[] {'#', '/', '^'}) == 0;
        }

        private static bool IsBlockHelperOrInversion(HelperExpression helperExpression)
        {
            if (helperExpression == null) return false;

            return helperExpression.HelperName.StartsWith("#") || (helperExpression.HelperName == "else");
        }
    }
}