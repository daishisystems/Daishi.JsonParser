#region Includes

using System.Linq;
using NUnit.Framework;

#endregion

namespace Daishi.JsonParser.UnitTests {
    [TestFixture]
    internal class JsonParserTest {
        [Test]
        public void JsonParserInitialises() {
            var json = Resources.JSON;
            var stream = new DummyStreamWriter(json);

            Assert.DoesNotThrow(() => new DummyJsonParser(stream.Write(), @"lesson_plan"));
        }

        [Test]
        public void JsonParserParsesSpecifiedJsonPropertyName() {
            var json = Resources.JSON;
            var stream = new DummyStreamWriter(json);

            var jsonParser = new DummyJsonParser(stream.Write(), @"lesson_plan");
            jsonParser.Parse();

            const int expectedCount = 10;
            Assert.AreEqual(expectedCount, jsonParser.Result.Count);

            const string expectedExternalId = @"JY14E_LP_1.1.1.1";
            Assert.AreEqual(expectedExternalId, jsonParser.Result.First().Id);
        }

        [Test]
        public void JsonParserParsesSimpleProperties() {
            var json = Resources.JSON;
            var stream = new DummyStreamWriter(json);

            var jsonParser = new DummyJsonParser(stream.Write(), @"lesson_plan");
            jsonParser.Parse();

            const int expectedDuration = 90;
            const string expectedtitle = @"Journeys: Lesson 1, Day 1";

            var lessonPlan = jsonParser.Result.First();

            Assert.AreEqual(expectedDuration, lessonPlan.Duration);
            Assert.AreEqual(expectedtitle, lessonPlan.Title);
        }

        [Test]
        public void JsonParserParsesArrays() {
            var json = Resources.JSON;
            var stream = new DummyStreamWriter(json);

            var jsonParser = new DummyJsonParser(stream.Write(), @"lesson_plan");
            jsonParser.Parse();

            var lessonPlan = jsonParser.Result.First();
            Assert.AreEqual(4, lessonPlan.Levels.Count);
        }

        [Test]
        public void JsonParserExistsGraceFullyIfSpecifiedJsonPropertyNameCannotBeParsed() {
            var json = Resources.JSON;
            var stream = new DummyStreamWriter(json);

            var jsonParser = new DummyJsonParser(stream.Write(), @"incorrect_property");
            jsonParser.Parse();

            Assert.AreEqual(0, jsonParser.Result.Count);
        }

        [Test]
        public void JsonParserDoesNotEnterInfiniteLoopIfSpecifiedPropertyContainsNoProperties() {
            var json = Resources.Invalid;
            var stream = new DummyStreamWriter(json);

            var jsonParser = new DummyJsonParser(stream.Write(), @"lesson_plan");
            jsonParser.Parse();

            Assert.AreEqual(10, jsonParser.Result.Count);
        }
    }
}