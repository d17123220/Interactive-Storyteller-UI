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
    public class StoryModel : PageModel
    {

        public string StoryText { get; set; }

        public string ErrorText { get; set; }

        [BindProperty]
        public string UserInput { get; set; }

        private IStorytellerAPIService _storytllerAPI;
        private UserManager<IdentityUser> _userManager;

        public StoryModel(IStorytellerAPIService storytllerAPI, UserManager<IdentityUser> userManager)
        {
            _storytllerAPI = storytllerAPI;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            // recover sessionID                
            var sessionID = HttpContext.Session.GetString("SessionID");
            if (string.IsNullOrEmpty(sessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("Index");
            }

            // Check if session variable is empty
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("SessionText")))
            {
                // Check API if there is text generated for this session
                StoryText = null;
            }
            else
            {
                // get story text from session
                StoryText = HttpContext.Session.GetString("SessionText");
            }
            
            return Page();
        }

        public IActionResult OnPost()
        {
            // recover sessionID                
            var sessionID = HttpContext.Session.GetString("SessionID");
            if (string.IsNullOrEmpty(sessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("Index");
            }

            // User posted some content
            // Check with API if content is valid
            bool checkContent = true;

            if (String.IsNullOrEmpty(UserInput))
            {
                // User didn't provide any input, so nothing to do
                return Page();            
            }
            else if (checkContent)
            {
                // Use API call to send new content to GPT model
                string apiContent = "!!API!!";

                // Add new text recieved from API to text and session
                string newContent = HttpContext.Session.GetString("SessionText")+ "\n\n" + UserInput + "\n\n" + apiContent;
                
                // Update session variable with user input and content from API
                HttpContext.Session.SetString("SessionText", newContent);
                
                // Apply new content to page model
                StoryText = newContent;

                // remove user input and any errors as successfull
                UserInput = null;
                ErrorText = null;
            }
            else
            {
                // Add error message
                ErrorText = "Please enter correct text!";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostFinish(string FinishNow)
        {
            // recover sessionID                
            var sessionID = HttpContext.Session.GetString("SessionID");
            if (string.IsNullOrEmpty(sessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("Index");
            }

            // Check if correctly passing data
            if (FinishNow.Equals("YesFinishNow"))
            {
                string userName = null;
                // get email address of the user
                var user = await _userManager.GetUserAsync(User);
                if (null != user)
                    userName = user.Email;
                else
                    // for debug purposes, use temporary email
                    userName = "demo@email.address";

                // Send a call to API to finish session
                await _storytllerAPI.DeleteSession(userName, sessionID);

                // Remove session variables
                HttpContext.Session.Clear();
                // redirect back to starting page
                return RedirectToPage("Index");
            }
            else
            {
                // For some reason variable passed after button pressed was not in correct format
                // Return to Story
                return Page();
            }
        }

    }

}