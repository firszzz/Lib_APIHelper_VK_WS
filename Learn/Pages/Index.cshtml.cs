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
        public string post_text;
        public string likes = "1";
        public string comments = "2";
        public string reposts = "10";
        public string name;
        public string screen_name;
        /*public string description;*/
        public string[] desc_arr;
        public string post_count;
        public string post_likes;
        public string members;
        public string best_post_dt;
        public string best_post_img_src;

        APIHelper apiHelper = new APIHelper(token);

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Group_Item[] stats_group = await apiHelper.get_stats(client);
            img_src = stats_group[0].photo_200.ToString();
            name = stats_group[0].name.ToString();
            /*description = stats_group[0].description.ToString();*/
            /*desc_arr = description.Split("\n");
            desc_arr = desc_arr.Where(x => x != "").ToArray();*/
            screen_name = "https://vk.com/" + stats_group[0].screen_name.ToString();
            post_count = await apiHelper.get_posts_json(client);
            JObject json = JObject.Parse(post_count);
            post_count = json["response"]["count"].ToString();
            members = stats_group[0].members_count.ToString();
            post_likes = (await apiHelper.get_likes_sum(client)).ToString();
            int[] bp = await apiHelper.get_top_reposted(client);
            string best_post_id = bp[0].ToString();
            Post_Item[] best_post = await apiHelper.get_post(client, best_post_id);
            post_text = best_post[0].text;
            int int_best_post_dt = best_post[0].date;
            best_post_dt = UnixTimeStampToDateTime(float.Parse(int_best_post_dt.ToString())).ToString();
            foreach (Attachment item in best_post[0].attachments)
            {
                if (item.type == "photo")
                {
                    best_post_img_src = item.photo.sizes[2].url;
                }
            }

            return Page();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}