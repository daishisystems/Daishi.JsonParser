#region Includes

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

#endregion

namespace Daishi.JsonParser.UnitTests {
    internal class DummyJsonParser : JsonParser<DummyParsable> {
        public DummyJsonParser(Stream json, string jsonPropertyName) : base(json, jsonPropertyName) {}

        protected override void Build(DummyParsable parsable, JsonTextReader reader) {
            if (reader.Value != null && reader.Value.Equals(@"id")) {
                reader.Read();
                parsable.Id = (string) reader.Value;
            }
            else if (reader.Value != null && reader.Value.Equals(@"duration")) {
                reader.Read();
                parsable.Duration = Convert.ToInt32(reader.Value);
            }
            else if (reader.Value != null && reader.Value.Equals(@"title")) {
                reader.Read();
                parsable.Title = (string) reader.Value;
            }
            else if (reader.Value != null && reader.Value.Equals(@"levels")) {
                do {
                    reader.Read();
                } while (!reader.TokenType.Equals(JsonToken.PropertyName) && !reader.TokenType.Equals(JsonToken.None));

                parsable.Levels = new List<DummyLevel>();

                do {
                    var levelType = (string) reader.Value;
                    reader.Read();

                    var levelValue = (string) reader.Value;
                    reader.Read();

                    parsable.Levels.Add(new DummyLevel {Type = levelType, Value = levelValue});
                } while (reader.TokenType != JsonToken.EndObject && !reader.TokenType.Equals(JsonToken.None));
            }
        }

        protected override bool IsBuilt(DummyParsable parsable, JsonTextReader reader) {
            var isBuilt = !string.IsNullOrEmpty(parsable.Id) &&
                          !string.IsNullOrEmpty(parsable.Title) &&
                          parsable.Duration > 0 &&
                          parsable.Levels != null &&
                          parsable.Levels.Count > 0;

            return isBuilt || base.IsBuilt(parsable, reader);
        }
    }
}