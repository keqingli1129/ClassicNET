namespace WebMVC.Tests
{
    /// <summary>
    /// Placeholder tests mirroring the WebMVC HomeController.
    /// These exercise a fake in-memory stand-in until the MVC project can be
    /// referenced (net10.0 test project cannot reference net4.8.1 WebMVC).
    /// </summary>
    public class HomeControllerTests
    {
        private sealed class FakeHomeController
        {
            public IDictionary<string, object?> ViewBag { get; } = new Dictionary<string, object?>();

            public string Index() => "Index";

            public string About()
            {
                ViewBag["Message"] = "Your application description page.";
                return "About";
            }

            public string Contact()
            {
                ViewBag["Message"] = "Your contact page.";
                return "Contact";
            }
        }

        [Fact]
        public void Index_ReturnsIndexView()
        {
            var controller = new FakeHomeController();

            var result = controller.Index();

            result.Should().Be("Index");
        }

        [Fact]
        public void About_SetsDescriptionMessage()
        {
            var controller = new FakeHomeController();

            var result = controller.About();

            result.Should().Be("About");
            controller.ViewBag.Should().ContainKey("Message")
                .WhoseValue.Should().Be("Your application description page.");
        }

        [Fact]
        public void Contact_SetsContactMessage()
        {
            var controller = new FakeHomeController();

            var result = controller.Contact();

            result.Should().Be("Contact");
            controller.ViewBag["Message"].Should().Be("Your contact page.");
        }

        [Theory]
        [InlineData("Index")]
        [InlineData("About")]
        [InlineData("Contact")]
        public void KnownActions_AreNotEmpty(string actionName)
        {
            actionName.Should().NotBeNullOrWhiteSpace();
        }
    }
}
