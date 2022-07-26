using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using vkAPIhelper;

namespace Learn.Pages
{
    public class IndexModel : PageModel
    {
        static string token = "vk1.a.oLu7wB601hwjywt60HJ2wDwARC7KLw25PvdwfG7Bs5KlKc3In7EazrVpKbDuBKCpA53RCcdBsci_CLwxu68_41E8e_RTCEr8yqR2YPtfxXCliMD0CjZWB04pyzC7oo7To9xuaRpPBze4g4MP1oCwqEHNxlHcjfZlU6qCWMyJwDVRsbyNB1f_kguEeNS5tXRo";
        HttpClient client = new HttpClient();
        private readonly ILogger<IndexModel> _logger;
        public string img_src = "https://icons.getbootstrap.com/assets/img/icons-hero.png";
        public string post_text = "Это текст поста, а значит его нужно добавить где-то сверху, чтобы было красиво.";
        public string likes = "1";
        public string comments = "2";
        public string reposts = "10";

        APIHelper apiHelper = new APIHelper(token);

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Group_Item[] stats_group = await apiHelper.get_stats(client);
            img_src = stats_group[0].photo_200.ToString();
            return Page();
        }
    }
}