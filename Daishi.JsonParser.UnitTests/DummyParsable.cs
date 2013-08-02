#region Includes

using System.Collections.Generic;

#endregion

namespace Daishi.JsonParser.UnitTests {
    internal class DummyParsable {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public List<DummyLevel> Levels { get; set; }
    }
}