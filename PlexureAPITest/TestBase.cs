using NUnit.Framework;

namespace PlexureAPITest
{
    [SetUpFixture]
    public class TestBase
    {
        protected EnhancedService Service;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Service = new EnhancedService();
        }

        [OneTimeTearDown]
        public void GlobalCleanup()
        {
            Service?.Dispose();
        }
    }
}
