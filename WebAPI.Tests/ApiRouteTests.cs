namespace WebAPI.Tests
{
    /// <summary>
    /// Placeholder tests for the default Web API route conventions.
    /// </summary>
    public class ApiRouteTests
    {
        private const string DefaultApiRouteTemplate = "api/{controller}/{id}";

        [Fact]
        public void DefaultApiRoute_StartsWithApiPrefix()
        {
            Assert.StartsWith("api/", DefaultApiRouteTemplate);
        }

        [Fact]
        public void DefaultApiRoute_ContainsControllerPlaceholder()
        {
            Assert.Contains("{controller}", DefaultApiRouteTemplate);
        }

        [Theory]
        [InlineData("api/values", "values", null)]
        [InlineData("api/values/5", "values", 5)]
        public void RouteParsing_ExtractsControllerAndId(string path, string expectedController, int? expectedId)
        {
            var segments = path.Split('/');

            Assert.Equal("api", segments[0]);
            Assert.Equal(expectedController, segments[1]);

            int? id = segments.Length > 2 ? int.Parse(segments[2]) : null;
            Assert.Equal(expectedId, id);
        }
    }
}
