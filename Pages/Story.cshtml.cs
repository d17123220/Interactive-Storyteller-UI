using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Interactive_Storyteller_UI.Services;
using Interactive_Storyteller_UI.Models;


namespace Interactive_Storyteller_UI.Pages
{
    [Authorize]
    public class StoryModel : PageModel
    {

        public string StoryText { get; set; }

        public string ErrorText { get; set; }

        [BindProperty]
        public string UserInput { get; set; }

        public HashSet<string> OffensiveTerms { get; set; }

        private IStorytellerAPIService _storytllerAPI;
        private UserManager<IdentityUser> _userManager;
        private string userName;

        public StoryModel(IStorytellerAPIService storytllerAPI, UserManager<IdentityUser> userManager)
        {
            _storytllerAPI = storytllerAPI;
            _userManager = userManager;
            OffensiveTerms = new HashSet<string>();

        }

        public async Task<IActionResult> OnGetAsync()
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
                // get email address of the user
                var user = await _userManager.GetUserAsync(User);
                if (null != user)
                    userName = user.Email;
                else
                    // for debug purposes, use temporary email
                    userName = "demo@email.address";
                
                // Check API if there is text generated for this session
                var textSoFar = await _storytllerAPI.GetContextForSession(userName, sessionID);
                if (null != textSoFar && textSoFar.Any())
                {
                    StoryText = "Story so far:";
                    foreach (var context in textSoFar)
                    {
                        StoryText += "\n\n" + context.SessionText;
                    }
                }
                else
                    StoryText = null;

            }
            else
                // get story text from session
                StoryText = HttpContext.Session.GetString("SessionText");
            
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // recover sessionID                
            var sessionID = HttpContext.Session.GetString("SessionID");
            if (string.IsNullOrEmpty(sessionID))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("Index");
            }

            // User posted some content
            if (String.IsNullOrEmpty(UserInput))
            {
                // User didn't provide any input, so nothing to do
                return Page();            
            }
            else
            {
                // Check with API if content is valid
                // and Use API call to send this content to GPT model
                var context = await _storytllerAPI.AddUserContext(UserInput, sessionID);

                // Update input field with corrected text
                ScreenedContext userContext = (ScreenedContext) context["ScreenedContext"];

                UserInput = userContext.CorrectedText;

                if (!userContext.IsBounced)
                {
                    Context apiContext = (Context) context["APIContext"];
                    // Add new text recieved from API to text and session
                    string newContent = HttpContext.Session.GetString("SessionText")+ "\n\n" + UserInput + "\n\n" + apiContext.SessionText;
                    
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
                    // set list of terms which are considered to be offensive
                    OffensiveTerms = userContext.OffensiveTerms.ToHashSet();
                    
                    // Return back stored in session text
                    StoryText =  HttpContext.Session.GetString("SessionText");

                    // Add error message
                    ErrorText = "Please enter correct text!";
                }
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