#region Includes

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

#endregion

namespace Daishi.JsonParser {
    public abstract class JsonParser<TParsable> : IParser<TParsable> where TParsable : class, new() {
        private readonly Stream json;
        private readonly string jsonPropertyName;

        public IEnumerable<TParsable> Result { get; private set; }

        protected JsonParser(Stream json, string jsonPropertyName) {
            this.json = json;
            this.jsonPropertyName = jsonPropertyName;
        }

        protected abstract void Build(TParsable parsable, JsonTextReader reader);

        protected virtual bool IsBuilt(TParsable parsable, JsonTextReader reader) {
            return reader.TokenType.Equals(JsonToken.None);
        }

        public void Parse() {
            using (var streamReader = new StreamReader(json)) {
                using (var jsonReader = new JsonTextReader(streamReader)) {
                    var result = new List<TParsable>();

                    do {
                        jsonReader.Read();
                        if (jsonReader.Value == null || !jsonReader.Value.Equals(jsonPropertyName)) continue;

                        var parsable = new TParsable();

                        do {
                            jsonReader.Read();
                        } while (!jsonReader.TokenType.Equals(JsonToken.PropertyName) && !jsonReader.TokenType.Equals(JsonToken.None));

                        do {
                            Build(parsable, jsonReader);
                            jsonReader.Read();
                        } while (!IsBuilt(parsable, jsonReader));

                        result.Add(parsable);
                    } while (!jsonReader.TokenType.Equals(JsonToken.None));

                    Result = result;
                }
            }
        }
    }
}