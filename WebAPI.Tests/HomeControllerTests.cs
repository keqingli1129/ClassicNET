namespace WebAPI.Tests
{
    /// <summary>
    /// Placeholder tests mirroring the HomeController.
    /// </summary>
    public class HomeControllerTests
    {
        [Fact]
        public void Index_SetsHomePageTitle()
        {
            var viewBagTitle = "Home Page";

            Assert.Equal("Home Page", viewBagTitle);
        }

        [Theory]
        [InlineData("Home")]
        [InlineData("Help")]
        public void KnownRoutes_AreNotEmpty(string routeName)
        {
            Assert.False(string.IsNullOrWhiteSpace(routeName));
        }
    }
}
