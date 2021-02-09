namespace Interactive_Storyteller_UI.Services
{

    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IStorytellerAPIService
    {
        Task<bool> CheckSession(string userName, string sessionID);
        Task<string> CreateNewSession(string userName);
        Task DeleteSession(string userName, string sessionID);

    }
}