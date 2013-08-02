#region Includes

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

#endregion

namespace Daishi.JsonParser {
    public abstract class JsonParser<T> where T : class, new() {
        private readonly Stream json;
        private readonly string jsonPropertyName;

        public List<T> Result { get; private set; }

        protected JsonParser(Stream json, string jsonPropertyName) {
            this.json = json;
            this.jsonPropertyName = jsonPropertyName;

            Result = new List<T>();
        }

        protected abstract void Build(T parsable, JsonTextReader reader);

        protected virtual bool IsBuilt(T parsable, JsonTextReader reader) {
            return reader.TokenType.Equals(JsonToken.None);
        }

        public void Parse() {
            using (var streamReader = new StreamReader(json)) {
                using (var jsonReader = new JsonTextReader(streamReader)) {
                    do {
                        jsonReader.Read();
                        if (jsonReader.Value == null || !jsonReader.Value.Equals(jsonPropertyName)) continue;

                        var parsable = new T();

                        do {
                            jsonReader.Read();
                        } while (!jsonReader.TokenType.Equals(JsonToken.PropertyName) && !jsonReader.TokenType.Equals(JsonToken.None));

                        do {
                            Build(parsable, jsonReader);
                            jsonReader.Read();
                        } while (!IsBuilt(parsable, jsonReader));

                        Result.Add(parsable);
                    } while (!jsonReader.TokenType.Equals(JsonToken.None));
                }
            }
        }
    }
}