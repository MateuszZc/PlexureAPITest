using NUnit.Framework;
using System.Net;

namespace PlexureAPITest.Tests
{
    [TestFixture]
    public class PurchaseTests : TestBase
    {
        [SetUp]
        public void Setup()
        {
            var loginResponse = Service.Login("Tester", "Plexure123");
            loginResponse.Expect(HttpStatusCode.OK);
        }

        [Test] // Purchase with valid credentials
        public void TEST_005_Purchase_Valid_Product()
        {
            var response = Service.Purchase(1);
            response.Expect(HttpStatusCode.Accepted);
            Assert.AreEqual("Purchase completed.", response.Entity.Message);
        }

        [Test] // Negative test - purchase without login
        public void TEST_006_Purchase_Without_Login()
        {
            Service.UpdateAuthenicationToken(string.Empty);
            var response = Service.Purchase(1);
            response.Expect(HttpStatusCode.Unauthorized);
        }

        [Test] // Negative test - purchase with invalid product ID
        public void TEST_007_Purchase_Invalid_Product()
        {
            var response = Service.Purchase(-1);
            response.Expect(HttpStatusCode.BadRequest);
        }

        [Test] // Verify 100 points added after successful purchase
        public void TEST_008_Verify_Points_After_Purchase()
        {
            var purchaseResponse = Service.Purchase(1);
            purchaseResponse.Expect(HttpStatusCode.Accepted);
            Assert.AreEqual(100, purchaseResponse.Entity.Points);
        }
    }
}
