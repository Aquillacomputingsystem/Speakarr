using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NzbDrone.Core.ThingiProvider;
using Speakarr.Api.V1.Indexers;
using Speakarr.Http.ClientSchema;

namespace NzbDrone.Integration.Test.ApiTests
{
    [TestFixture]
    public class IndexerFixture : IntegrationTest
    {
        [Test]
        [Ignore("Need mock Newznab to test")]
        public void should_have_built_in_indexer()
        {
            var indexers = Indexers.All();

            indexers.Should().NotBeEmpty();
            indexers.Should().NotContain(c => string.IsNullOrWhiteSpace(c.Name));
            indexers.Where(c => c.ConfigContract == typeof(NullConfig).Name).Should().OnlyContain(c => c.EnableRss);
        }

        private IndexerResource GetNewznabSchemav1(string name = null)
        {
            var schema = Indexers.Schema().First(v => v.Implementation == "Newznab");

            schema.Name = name;
            schema.EnableRss = false;
            schema.EnableAutomaticSearch = false;
            schema.EnableInteractiveSearch = false;

            return schema;
        }

        private Field GetCategoriesField(IndexerResource resource)
        {
            var field = resource.Fields.First(v => v.Name == "categories");

            return field;
        }

        [Test]
        public void all_preset_fields_should_be_set_correctly()
        {
            var schema = GetNewznabSchemav1();

            schema.Presets.Any(x => x.SupportsRss).Should().BeTrue();
        }

        [Test]
        public void v2_categories_should_be_array()
        {
            var schema = GetNewznabSchemav1();

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value.Should().BeOfType<JArray>();
        }

        [Test]
        public void v3_categories_should_be_array()
        {
            var schema = GetNewznabSchemav1();

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value.Should().BeOfType<JArray>();
        }

        [Test]
        public void v2_categories_should_accept_null()
        {
            var schema = GetNewznabSchemav1("Testv2null");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = null;

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().Should().BeEmpty();
        }

        [Test]
        public void v2_categories_should_accept_emptystring()
        {
            var schema = GetNewznabSchemav1("Testv2emptystring");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = "";

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().Should().BeEmpty();
        }

        [Test]
        public void v2_categories_should_accept_string()
        {
            var schema = GetNewznabSchemav1("Testv2string");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = "1000,1010";

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().ToObject<int[]>().Should().BeEquivalentTo(new[] { 1000, 1010 });
        }

        [Test]
        public void v2_categories_should_accept_array()
        {
            var schema = GetNewznabSchemav1("Testv2array");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = new object[] { 1000, 1010 };

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().ToObject<int[]>().Should().BeEquivalentTo(new[] { 1000, 1010 });
        }

        [Test]
        public void v3_categories_should_accept_null()
        {
            var schema = GetNewznabSchemav1("Testv3null");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = null;

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().Should().BeEmpty();
        }

        [Test]
        public void v3_categories_should_accept_emptystring()
        {
            var schema = GetNewznabSchemav1("Testv3emptystring");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = "";

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().Should().BeEmpty();
        }

        [Test]
        public void v3_categories_should_accept_string()
        {
            var schema = GetNewznabSchemav1("Testv3string");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = "1000,1010";

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().ToObject<int[]>().Should().BeEquivalentTo(new[] { 1000, 1010 });
        }

        [Test]
        public void v3_categories_should_accept_array()
        {
            var schema = GetNewznabSchemav1("Testv3array");

            var categoriesField = GetCategoriesField(schema);

            categoriesField.Value = new object[] { 1000, 1010 };

            var result = Indexers.Post(schema);

            var resultArray = GetCategoriesField(result).Value;
            resultArray.Should().BeOfType<JArray>();
            resultArray.As<JArray>().ToObject<int[]>().Should().BeEquivalentTo(new[] { 1000, 1010 });
        }
    }
}
