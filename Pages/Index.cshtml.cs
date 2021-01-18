using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Interactive_Storyteller_UI.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Check if session already exists in API
            // If yes, redirect to text

            // if no, show prompt
        }

        public IActionResult OnPost(string StartNew)
        {
            if (StartNew.Equals("YesStartIt"))
            {
                // send API call to start new

                // Recieve API session ID, and store it in session
                // var session = API_CALL(username = username)
                string session = "";
                HttpContext.Session.SetString("SessionID", session);

                // redirect to Story page
                return RedirectToPage("Story");
            }
            else
            {
                // For some reason variable passed after button pressed was not in correct format
                // Return to index/home page
                return RedirectToPage("Index");
            }
        }
    }
}
