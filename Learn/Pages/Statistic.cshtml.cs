using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Learn.Pages
{
    public class StatisticModel : PageModel
    {
        public string? Statistic { get; set; }

        readonly HttpClient Client = new();
        readonly string key = "vk1.a.oLu7wB601hwjywt60HJ2wDwARC7KLw25PvdwfG7Bs5KlKc3In7EazrVpKbDuBKCpA53RCcdBsci_CLwxu68_41E8e_RTCEr8yqR2YPtfxXCliMD0CjZWB04pyzC7oo7To9xuaRpPBze4g4MP1oCwqEHNxlHcjfZlU6qCWMyJwDVRsbyNB1f_kguEeNS5tXRo";
        readonly string type = "post";
        readonly string owner_id = "239328903";
        public string item_id = "911";

        public async Task<string> Get_response()
        {
            string request = "https://api.vk.com/method/likes.add?" + $"type={type}&owner_id={owner_id}&item_id={item_id}&" + $"access_token={key}" + "&v=5.131";

            string responseTask = await Client.GetStringAsync(request);

            JObject json = JObject.Parse(responseTask);

            return json["response"]["likes"].ToString();
        }

        public async Task<IActionResult>  OnGetAsync()
        {
            Statistic = await Get_response();
            return Page();
        }
    }
}
