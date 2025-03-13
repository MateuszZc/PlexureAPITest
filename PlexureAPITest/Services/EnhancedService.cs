using Newtonsoft.Json;
using PlexureAPITest.Config;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PlexureAPITest
{
    public class EnhancedService : IDisposable
    {
        HttpClient client;

        public EnhancedService()
        {
            client = new HttpClient { BaseAddress = new Uri("https://qatestapi.azurewebsites.net") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("token", TestConfig.RequestHeaders.DefaultAuthenticationToken.HeaderValue);
        }

        public void UpdateAuthenicationToken(string authToken)
        {
            if (client.DefaultRequestHeaders.Contains("token"))
                client.DefaultRequestHeaders.Remove("token");

            client.DefaultRequestHeaders.Add("token", authToken);
        }

        private StringContent CreateJsonContent(Dictionary<string, object> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public Response<UserEntity> Login(string username, string password)
        {
            var content = CreateJsonContent(new Dictionary<string, object>
            {
                {"UserName", username},
                {"Password", password}
            });

            using (var response = client.PostAsync("api/login", content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<UserEntity>(response.Content.ReadAsStringAsync().Result);
                    UpdateAuthenicationToken(user.AccessToken);
                    return new Response<UserEntity>(response.StatusCode, user);
                }
                return new Response<UserEntity>(response.StatusCode, response.Content.ReadAsStringAsync().Result);
            }
        }

        public Response<PurchaseEntity> Purchase(int productId)
        {
            var content = CreateJsonContent(new Dictionary<string, object> { { "ProductId", productId } });

            using (var response = client.PostAsync("api/purchase", content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var purchase = JsonConvert.DeserializeObject<PurchaseEntity>(response.Content.ReadAsStringAsync().Result);
                    return new Response<PurchaseEntity>(response.StatusCode, purchase);
                }
                return new Response<PurchaseEntity>(response.StatusCode, response.Content.ReadAsStringAsync().Result);
            }
        }

        public Response<PointsEntity> GetPoints()
        {
            using (var response = client.GetAsync($"api/points").Result)
            {
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    var points = JsonConvert.DeserializeObject<PointsEntity>(response.Content.ReadAsStringAsync().Result);
                    return new Response<PointsEntity>(response.StatusCode, points);
                }
                return new Response<PointsEntity>(response.StatusCode, response.Content.ReadAsStringAsync().Result);
            }
        }

        public void Dispose() => client?.Dispose();
    }
}
