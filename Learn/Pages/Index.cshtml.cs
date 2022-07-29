using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using vkAPIhelper;

namespace Learn.Pages
{
    public class IndexModel : PageModel
    {
        static string token = "vk1.a.oLu7wB601hwjywt60HJ2wDwARC7KLw25PvdwfG7Bs5KlKc3In7EazrVpKbDuBKCpA53RCcdBsci_CLwxu68_41E8e_RTCEr8yqR2YPtfxXCliMD0CjZWB04pyzC7oo7To9xuaRpPBze4g4MP1oCwqEHNxlHcjfZlU6qCWMyJwDVRsbyNB1f_kguEeNS5tXRo";
        public string? img_src;
        public string? post_text;
        public string? name;
        public string? screen_name;
        public string[]? desc_arr;
        public string? post_count;
        public string? post_likes;
        public string? members;
        public string? best_post_dt;
        public string? best_post_img_src;
        public string? best_post_likes_count;
        public string? best_post_reposts_count;
        public string? best_post_comments_count;
        public string? best_post_views_count;

        readonly HttpClient client = new();
        readonly APIHelper apiHelper = new(token);

        public async Task<IActionResult> OnGetAsync()
        {
            int[] bp = await apiHelper.get_top_reposted(client);
            string best_post_id = bp[0].ToString();
            string response_post_count = await apiHelper.get_posts_json(client);

            post_likes = (await apiHelper.get_likes_sum(client)).ToString();

            List<Group_Item> stats_group = await apiHelper.get_stats(client);
            List<Post_Item> best_post = await apiHelper.get_post(client, best_post_id);
            Likes bp_likes = best_post[0].likes;
            Reposts bp_reposts = best_post[0].reposts;
            Views bp_views = best_post[0].views;

            JObject json = JObject.Parse(response_post_count);

            int int_best_post_dt = best_post[0].date;
            img_src = stats_group[0].photo_200.ToString();
            name = stats_group[0].name.ToString();
            screen_name = "https://vk.com/" + stats_group[0].screen_name.ToString();
            post_count = json["response"]["count"].ToString();
            members = stats_group[0].members_count.ToString();
            post_text = best_post[0].text;
            best_post_dt = UnixTimeStampToDateTime(float.Parse(int_best_post_dt.ToString())).ToString();

            var sizes_bp = new List<int>();
            int max_size_bp = 0;

            foreach (Attachment item in best_post[0].attachments)
            {
                if (item.type == "photo")
                {
                    foreach (var size in item.photo.sizes)
                    {
                        sizes_bp.Add(size.width);
                    }
                }
            }

            int[] sizes_bp_array = new int[sizes_bp.Count];
            sizes_bp_array = sizes_bp.ToArray();
            max_size_bp = sizes_bp.Max();
            int index = Array.FindLastIndex(sizes_bp_array, delegate (int i) { return i == max_size_bp; });

            foreach (Attachment item in best_post[0].attachments)
            {
                if (item.type == "photo")
                {
                    best_post_img_src = item.photo.sizes[index].url;
                }
            }

            best_post_likes_count = bp_likes.count.ToString();
            best_post_reposts_count = bp_reposts.count.ToString();
            best_post_views_count = bp_views.count.ToString();

            return Page();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}