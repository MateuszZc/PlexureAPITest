using NUnit.Framework;
using System.Net;

namespace PlexureAPITest.Tests
{
    [TestFixture]
    public class PointsTests : TestBase
    {
        [SetUp]
        public void Setup()
        {
            var loginResponse = Service.Login("Tester", "Plexure123");
            loginResponse.Expect(HttpStatusCode.OK);
        }

        [Test] // Get Points with Valid Product
        public void TEST_009_Get_Points_Valid_Product()
        {
            var response = Service.GetPoints();
            response.Expect(HttpStatusCode.Accepted);
        }

        [Test] // Negative Test - Unauthorized Access for Points
        public void TEST_010_Get_Points_Unauthorized()
        {
            Service.UpdateAuthenicationToken("InvalidToken");
            var response = Service.GetPoints();
            response.Expect(HttpStatusCode.Unauthorized);
        }
    }
}
