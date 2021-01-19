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
    public class StoryModel : PageModel
    {

        public string StoryText { get; set; }

        public string ErrorText { get; set; }

        [BindProperty]
        public string UserInput { get; set; }

        public StoryModel()
        {

        }

        public IActionResult OnGet()
        {
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

        public IActionResult OnPostFinish(string FinishNow)
        {
            if (FinishNow.Equals("YesFinishNow"))
            {
                // Send a call to API to finish session

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