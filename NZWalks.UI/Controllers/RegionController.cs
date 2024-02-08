using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public async Task <IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                // Get All region from Web api
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7151/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                var stringResponseBody = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();
                ViewBag.Response = stringResponseBody;
            }
            catch (Exception ex)
            {

                //
            }
            return View(response);
        }
    }
}
