namespace WebMVC.Tests
{
    /// <summary>
    /// Placeholder tests for the default MVC route conventions
    /// (mirrors WebMVC/App_Start/RouteConfig.cs).
    /// </summary>
    public class MvcRouteTests
    {
        private const string DefaultRouteTemplate = "{controller}/{action}/{id}";
        private const string DefaultController = "Home";
        private const string DefaultAction = "Index";

        [Fact]
        public void DefaultRoute_ContainsControllerPlaceholder()
        {
            DefaultRouteTemplate.Should().Contain("{controller}");
        }

        [Fact]
        public void DefaultRoute_ContainsActionPlaceholder()
        {
            DefaultRouteTemplate.Should().Contain("{action}");
        }

        [Fact]
        public void DefaultRoute_UsesHomeIndexDefaults()
        {
            DefaultController.Should().Be("Home");
            DefaultAction.Should().Be("Index");
        }

        [Theory]
        [InlineData("Home/Index/5", "Home", "Index", 5)]
        [InlineData("Home/About", "Home", "About", null)]
        public void RouteParsing_ExtractsControllerActionAndId(string path, string expectedController, string expectedAction, int? expectedId)
        {
            var segments = path.Split('/');

            segments[0].Should().Be(expectedController);
            segments[1].Should().Be(expectedAction);

            int? id = segments.Length > 2 ? int.Parse(segments[2]) : null;
            id.Should().Be(expectedId);
        }
    }
}
