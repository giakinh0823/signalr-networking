using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatAppGroup.Pages.Chat
{
    public class IndexModel : PageModel
    {
        [BindProperty(Name = "groupId", SupportsGet = true)]
        public string? GroupId { get; set; }

        public void OnGet([FromQuery] string user)
        {
            ViewData["user"] = user;
        }
    }
}
