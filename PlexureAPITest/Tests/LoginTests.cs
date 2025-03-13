using NUnit.Framework;
using System.Net;

namespace PlexureAPITest.Tests
{
    [TestFixture]
    public class LoginTests : TestBase
    {
        [Test] // Successful Login Test
        public void TEST_001_Login_With_Valid_Credentials()
        {
            var response = Service.Login("Tester", "Plexure123");
            response.Expect(HttpStatusCode.OK);
            Assert.IsNotNull(response.Entity.AccessToken);
        }

        [Test] // Negative Test - Invalid Credentials
        public void TEST_002_Login_Invalid_Credentials()
        {
            var response = Service.Login("InvalidUser", "WrongPass");
            response.Expect(HttpStatusCode.Unauthorized);
        }

        [Test] // Negative Test - Missing Username
        public void TEST_003_Login_Missing_Username()
        {
            var response = Service.Login("", "Plexure123");
            response.Expect(HttpStatusCode.BadRequest);
        }

        [Test] // Negative Test - Missing Password
        public void TEST_004_Login_Missing_Password()
        {
            var response = Service.Login("Tester", "");
            response.Expect(HttpStatusCode.BadRequest);
        }
    }
}
