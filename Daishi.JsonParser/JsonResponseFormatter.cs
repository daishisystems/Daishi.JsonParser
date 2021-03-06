﻿#region Includes

using System.Collections.Generic;

#endregion

namespace Daishi.JsonParser {
    public class JsonResponseFormatter {
        public IEnumerable<T> Format<T>(JsonParser<T> parser) where T : class, new() {
            parser.Parse();
            return parser.Result;
        }
    }
}