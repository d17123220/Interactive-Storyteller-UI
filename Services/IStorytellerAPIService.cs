namespace Interactive_Storyteller_UI.Services
{

    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Interactive_Storyteller_UI.Models;

    public interface IStorytellerAPIService
    {
        Task<bool> CheckSession(string userName, string sessionID);
        Task<string> CreateNewSession(string userName);
        Task DeleteSession(string userName, string sessionID);
        Task<ScreenedContext> CheckContext(string text);

    }
}