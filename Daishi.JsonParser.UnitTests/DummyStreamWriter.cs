#region Includes

using System.IO;
using System.Text;

#endregion

namespace Daishi.JsonParser.UnitTests {
    internal class DummyStreamWriter {
        private readonly string json;

        public DummyStreamWriter(string json) {
            this.json = json;
        }

        public Stream Write() {
            var bytes = Encoding.UTF8.GetBytes(json);
            return new MemoryStream(bytes);
        }
    }
}