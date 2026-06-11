namespace WebAPI.Tests
{
    /// <summary>
    /// Placeholder tests mirroring the ValuesController endpoints.
    /// These use a fake in-memory stand-in until the API project can be referenced.
    /// </summary>
    public class ValuesControllerTests
    {
        private sealed class FakeValuesController
        {
            private readonly List<string> _values = new() { "value1", "value2" };

            public IEnumerable<string> Get() => _values;

            public string Get(int id) => "value";

            public void Post(string value) => _values.Add(value);

            public void Put(int id, string value)
            {
                if (id >= 0 && id < _values.Count)
                {
                    _values[id] = value;
                }
            }

            public void Delete(int id)
            {
                if (id >= 0 && id < _values.Count)
                {
                    _values.RemoveAt(id);
                }
            }
        }

        [Fact]
        public void Get_ReturnsAllValues()
        {
            var controller = new FakeValuesController();

            var result = controller.Get();

            Assert.Equal(new[] { "value1", "value2" }, result);
        }

        [Fact]
        public void Get_WithId_ReturnsValue()
        {
            var controller = new FakeValuesController();

            var result = controller.Get(5);

            Assert.Equal("value", result);
        }

        [Fact]
        public void Post_AddsValue()
        {
            var controller = new FakeValuesController();

            controller.Post("value3");

            Assert.Contains("value3", controller.Get());
        }

        [Fact]
        public void Put_UpdatesValue()
        {
            var controller = new FakeValuesController();

            controller.Put(0, "updated");

            Assert.Equal("updated", controller.Get().First());
        }

        [Fact]
        public void Delete_RemovesValue()
        {
            var controller = new FakeValuesController();

            controller.Delete(0);

            Assert.Single(controller.Get());
        }
    }
}
