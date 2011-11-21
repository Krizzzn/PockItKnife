using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using PockItKnife;
using System.Web.Script.Serialization;
using System.Data;
using System.Reflection;

namespace PockItKnifeTest
{
    [TestFixture]
    public class ToJsonTest
    {
        [TestCase("")]
        [TestCase(null)]
        [Test]
        public void Convert__returns_empty_string_if_key_is_null_or_empty(string key)
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert(key, 54, typeof(int));

            //ASSERT
            result.Should().NotBeNull();
            result.Should().BeBlank();
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(string))]
        [TestCase(typeof(ToJson))]
        [Test]
        public void Convert__returns_key_value_for_null_value(Type theType)
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", null, theType);

            //ASSERT
            result.Should().Contain("bort").And.Contain("null").And.NotContain("'null").And.NotContain("\"null");
        }

        [Test]
        public void Convert__returns_key_value_for_string()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", "mork", typeof(string));

            //ASSERT
            result.Should().Contain("bort").And.Contain("mork").And.Contain(":");
        }

        [Test]
        public void Convert__returns_boolean_value()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", true, typeof(bool));

            //ASSERT
            result.Should().Contain("bort").And.Contain("true").And.Contain(":").And.NotContain("'true").And.NotContain("\"true");
        }

        [Test]
        [TestCase(typeof(double))]
        [TestCase(typeof(int))]
        [TestCase(typeof(short))]
        [TestCase(typeof(float))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(long))]
        [TestCase(typeof(ulong))]
        [TestCase(typeof(uint))]
        [TestCase(typeof(ushort))]
        public void Convert__returns_numeric_values_value(Type type)
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", 99845, type);

            //ASSERT
            result.Should().Contain("bort").And.Contain("99845").And.Contain(":").And.NotContain("'99845").And.NotContain("\"99845");
        }

        [Test]
        public void Convert__maintains_comma()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", 4.6, typeof(double));

            //ASSERT
            result.Should().Contain("bort").And.Contain("4.6").And.Contain(":").And.NotContain("'4.6").And.NotContain("\"4.6");
        }

        [Test]
        public void Convert__escape_delimiter()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", "bo'rt", typeof(string));

            //ASSERT
            result.Should().Contain("bort").And.Contain(@"bo\'rt").And.Contain(":").And.NotContain("bo'rt");
        }

        [Test]
        public void Convert__escapes_column_name()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", "weeeeeeeee", typeof(string));

            //ASSERT
            result.Should().Contain(@"'bort':");
        }

        [Test]
        public void Convert__returns_float_value()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.Convert("bort", 998435.0, typeof(float));

            //ASSERT
            result.Should().Contain("bort").And.Contain("998435").And.Contain(":").And.NotContain("'998435").And.NotContain("\"998435");
        }

        [TestCase("va", typeof(string))]
        [TestCase(null, typeof(string))]
        [TestCase(false, typeof(Boolean))]
        [TestCase(9.456, typeof(double))]
        [TestCase(6, typeof(int))]
        [Test(Description = "Integration")]
        public void Convert__produces_valid_json(object value, Type type)
        {
            // ARRANGE
            var j = new ToJson_Accessor();
            var serializer = new JavaScriptSerializer();

            //ACT
            string jsonString = j.Convert("bort", value, type);
            var result = serializer.DeserializeObject("{" + jsonString + "}");

            //ASSERT
            result.Should().NotBeNull();
        }

        [Test(Description = "Integration")]
        public void Convert__produces_valid_json_for_long_text()
        {
            // ARRANGE
            var j = new ToJson_Accessor();
            var serializer = new JavaScriptSerializer();
            var longTest = Assembly.GetExecutingAssembly().LoadEmbeddedFile("ToJsonTest.longtext");

            //ACT
            string jsonString = j.Convert("bort", longTest, typeof(string));
            var result = serializer.DeserializeObject("{" + jsonString + "}");

            //ASSERT
            result.Should().NotBeNull();
        }

        [Test(Description = "Integration")]
        public void Convert__produces_valid_json_for_datatable_one_row()
        {
            // ARRANGE
            var j = new ToJson_Accessor();
            var dt = new DataTable();
            var serializer = new JavaScriptSerializer();

            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("thefoo", typeof(string));

            var row = dt.NewRow();
            row[0] = 5;
            row[1] = "bort";
            dt.Rows.Add(row);

            //ACT
            var jsonString = j.ConvertDataTable(dt);
            var result = serializer.DeserializeObject(jsonString);

            //ASSERT
            result.Should().NotBeNull();
        }

        [Test(Description = "Integration")]
        public void Convert__produces_valid_json_for_datatable_multiple_rows()
        {
            // ARRANGE
            var j = new ToJson_Accessor();
            var dt = new DataTable();
            var serializer = new JavaScriptSerializer();

            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("thefoo", typeof(string));

            for (int i = 0; i < 5; i++) {
                var row = dt.NewRow();
                row[0] = i;
                row[1] = "bort {0}".Inject(i);
                dt.Rows.Add(row);
            }

            //ACT
            var jsonString = j.ConvertDataTable(dt);
            var result = serializer.DeserializeObject(jsonString) as object[];

            //ASSERT
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }

        [Test]
        public void ConvertDataTable__handle_null()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.ConvertDataTable(null);

            //ASSERT
            result.Should().Contain("[]");
        }

        [Test]
        public void ConvertDataTable__handle_empty()
        {
            // ARRANGE
            var j = new ToJson_Accessor();

            //ACT
            var result = j.ConvertDataTable(new DataTable());

            //ASSERT
            result.Should().Contain("[]");
        }

        [Test]
        public void ConvertDataTable__handle_empty_2()
        {
            // ARRANGE
            var j = new ToJson_Accessor();
            var dt = new DataTable();

            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("thefoo", typeof(string));

            //ACT
            var result = j.ConvertDataTable(new DataTable());

            //ASSERT
            result.Should().Contain("[]");
        }

        [Test]
        public void ConvertDataTable__converts_datatable_with_one_row()
        {
            // ARRANGE
            var j = new ToJson_Accessor();
            var dt = new DataTable();

            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("thefoo", typeof(string));

            var row = dt.NewRow();
            row[0] = 5;
            row[1] = "bort";
            dt.Rows.Add(row);

            //ACT
            var result = j.ConvertDataTable(dt);

            //ASSERT
            result.Should().Contain(@"[{").And.Contain("'id':").And.Contain("'thefoo':").And.Contain("'bort'").And.Contain(@"}]");
        }

        [Test]
        public void ConvertDataTable__converts_datatable_with_multiple_rows()
        {
            // ARRANGE
            var j = new ToJson_Accessor();
            var dt = new DataTable();

            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("thefoo", typeof(string));

            for (int i = 0; i < 5; i++) {
                var row = dt.NewRow();
                row[0] = i;
                row[1] = "bort {0}".Inject(i);
                dt.Rows.Add(row);
            }

            //ACT
            var result = j.ConvertDataTable(dt);

            //ASSERT
            result.Should().Contain(@"[{").And.Contain("},{").And.Contain(@"}]").And.Contain("bort 3");
        }
    }
}
