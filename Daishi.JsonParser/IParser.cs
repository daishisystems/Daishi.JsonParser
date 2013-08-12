#region Includes

using System.Collections.Generic;

#endregion

namespace Daishi.JsonParser {
    public interface IParser<out TParsable> where TParsable : class, new() {
        IEnumerable<TParsable> Result { get; }
        void Parse();
    }
}