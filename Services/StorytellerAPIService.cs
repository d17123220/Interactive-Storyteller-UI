namespace Interactive_Storyteller_UI.Services
{

    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text;
    using Interactive_Storyteller_UI.Models;
    

    public class StorytellerAPIService : IStorytellerAPIService
    {
        private readonly HttpClient _httpClient;
        
        public StorytellerAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckSession(string userName, string sessionID)
        {
            // use HttpClient to call for GET method to check if session exists
            var response = await _httpClient.GetAsync($"/api/Session/{userName}/{sessionID}");
            // ensure recieved successfull answer 
            response.EnsureSuccessStatusCode();

            // decode answer into stream and de-serialize it from JSON to bool
            using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<bool>(responseStream);
        }

        public async Task<string> CreateNewSession(string userName)
        {
            // prepare new session object (only username is neede though)
            var sessionRequest = new Session();
            sessionRequest.UserName = userName;
            
            // serialize object to JSON
            var tmp = JsonSerializer.Serialize(sessionRequest);
            var jsonRequest = new StringContent(JsonSerializer.Serialize(sessionRequest), Encoding.UTF8, "application/json");
            
            // use HttpClient to call for POST method and upload JSON object
            using var response = await _httpClient.PostAsync("/api/Session", jsonRequest);
            // ensure recieved succsessfull answer
            response.EnsureSuccessStatusCode();

            // decode answer into stream and de-serialize stream into Session object
            using var responseStream = await response.Content.ReadAsStreamAsync();
                var sessionResponse = await JsonSerializer.DeserializeAsync<Session>(responseStream);
            
            // return sessionID from recieved object 
            return sessionResponse.SessionID;
        }

        public async Task<bool> DeleteSession(string userName, string sessionID)
        {
            return false;
        }

    }
}