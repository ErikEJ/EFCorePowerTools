using System;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal enum TokenType
    {
        Static = 0,
        StartExpression = 1,
        EndExpression = 2,
        Word = 3,
        Literal = 4,
        Structure = 5,
        Comment = 6,
        Partial = 7,
        Layout = 8,
        StartSubExpression = 9,
        EndSubExpression = 10,
        HashParameter = 11
    }
}

