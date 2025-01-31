using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace YourNamespace.Controllers
{
    public class WebController : Controller
    {
        private readonly HttpClient _httpClient;

        public WebController()
        {
            _httpClient = new HttpClient();
        }

        // Action to display the input form
        public ActionResult InputEncryptedData()
        {
            return View();
        }

        // Action to handle form submission and call the API
        [HttpPost]
        public async Task<ActionResult> CallSynchronizeEndpoint(string plainEncryptedData)
        {
            if (string.IsNullOrEmpty(plainEncryptedData))
            {
                ViewBag.ResponseContent = "Encrypted data cannot be empty.";
                return View("InputEncryptedData");
            }

            string apiUrl = "https://devcaamobileapp1app1.azurewebsites.net/WebAuthentication/synchronize"; // Replace with your actual API URL
            string responseContent;

            try
            {
                // Serialize the input data
                var content = new StringContent(JsonConvert.SerializeObject(plainEncryptedData), Encoding.UTF8, "application/json");

                // Make the API call i didnt move tasks
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                // Read the response
                if (response.IsSuccessStatusCode)
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    responseContent = $"Error: {response.StatusCode}, {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                responseContent = $"Request error: {ex.Message}";
            }

            // Pass the response content to the View
            ViewBag.ResponseContent = responseContent;
            return View("InputEncryptedData");
        }
    }
}
