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

        public StoryModel()
        {

        }

        public IActionResult OnGet()
        {
            // Chekc if session variable is empty
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("SessionText")))
            {
                // Check API if there is text generated for this session 
            }
            else
            {
                // get story text from session
                StoryText = HttpContext.Session.GetString("SessionText");
            }
            
            return Page();
        }

        public void OnPost()
        {
            // User posted some content

            // CHeck with API if content is valid

                // Use API call to send new content to GPT model
                // Add new text recieved from API to text and session

                // Otherwise show error
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