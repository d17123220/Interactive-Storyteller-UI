namespace Interactive_Storyteller_UI.Services
{

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interactive_Storyteller_UI.Models;

    public interface IStorytellerAPIService
    {
        Task<bool> CheckSession(string userName, string sessionID);
        Task<string> CreateNewSession(string userName);
        Task DeleteSession(string userName, string sessionID);
        Task<ScreenedContext> CheckContext(string text);
        Task<Dictionary<string, Object>> AddUserContext(string userText, string sessionId);

    }
}