﻿using System.Collections.Generic;

namespace SqlSharpener
{
    public class DataType
    {
        public IDictionary<TypeFormat, string> Map { get; set; }
        public bool Nullable { get; set; }
    }
}
