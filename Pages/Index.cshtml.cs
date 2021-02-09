using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Interactive_Storyteller_UI.Services;


namespace Interactive_Storyteller_UI.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IStorytellerAPIService _storytllerAPI;
        private UserManager<IdentityUser> _userManager;

        public IndexModel(ILogger<IndexModel> logger, IStorytellerAPIService storytellerAPI, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _storytllerAPI = storytellerAPI;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if sessionID is stored in session store
            var sessionID = HttpContext.Session.GetString("SessionID");
            if (string.IsNullOrEmpty(sessionID))
            {
                // show prompt/page
                return Page();
            }
            else
            {
                string userName = null;
                // get email address of the user
                var user = await _userManager.GetUserAsync(User);
                if (null != user)
                    userName = user.Email;
                else
                    // for debug purposes, use temporary email
                    userName = "demo@email.address";

                // Check if session already exists in API
                var result =  await _storytllerAPI.CheckSession(userName, sessionID);
                // If yes, redirect to Story
                if (result)
                    return RedirectToPage("Story");
                else
                {
                    // if no such session exist for this use: clear session and show prompt
                    HttpContext.Session.Clear();
                    return Page();
                }
            }
        }

        public async Task<IActionResult> OnPost(string StartNew)
        {
            if (StartNew.Equals("YesStartIt"))
            {
                string userName = null;
                // get email address of the user
                var user = await _userManager.GetUserAsync(User);
                if (null != user)
                    userName = user.Email;
                else
                    // for debug purposes, use temporary email
                    userName = "demo@email.address";

                // send API call to start new session
                var sessionID = await _storytllerAPI.CreateNewSession(userName);

                // Recieve API sessionID, and store it in session
                HttpContext.Session.SetString("SessionID", sessionID);

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
