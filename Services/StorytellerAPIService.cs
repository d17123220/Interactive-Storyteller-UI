namespace Interactive_Storyteller_UI.Services
{

    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
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

        public async Task DeleteSession(string userName, string sessionID)
        {
            // use HttpClient to call for DELETE method to check if session exists
            using var response = await _httpClient.DeleteAsync($"/api/Session/{userName}/{sessionID}");
            // ensure recieved successfull answer 
            response.EnsureSuccessStatusCode();
        }

        public async Task<ScreenedContext> CheckContext(string userText)
        {
            Object userInput = new {text = userText};
            // generate a new request body
            var jsonRequest = new StringContent(JsonSerializer.Serialize(userInput), Encoding.UTF8, "application/json");
            
            // use HttpClient to call for POST method and upload TEXT object
            using var response = await _httpClient.PostAsync("/api/Context/Check/from-Interactive-Storyteller-UI", jsonRequest);
            // ensure recieved succsessfull answer
            response.EnsureSuccessStatusCode();

            // decode answer into stream and de-serialize stream into ScreenedContext object
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var sessionResponse = await JsonSerializer.DeserializeAsync<ScreenedContext>(responseStream);
            
            return sessionResponse;
        }

        public async Task<Dictionary<string, Object>> AddUserContext(string userText, string sessionId)
        {
            var returnHash = new Dictionary<string, Object>();
            var ScreenedContext = await CheckContext(userText);

            returnHash["ScreenedContext"] = ScreenedContext;
            if (!ScreenedContext.IsBounced)
            {            
                var userInput = new {text = userText, sessionID = sessionId};

                // generate a new request body
                var jsonRequest = new StringContent(JsonSerializer.Serialize(userInput), Encoding.UTF8, "application/json");
                
                // use HttpClient to call for POST method and upload TEXT object
                using var response = await _httpClient.PostAsync("/api/Context", jsonRequest);
                // ensure recieved succsessfull answer
                response.EnsureSuccessStatusCode();

                // decode answer into stream and de-serialize stream into ScreenedContext object
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var contextResponse = await JsonSerializer.DeserializeAsync<Context>(responseStream);
                
                returnHash["APIContext"] = contextResponse;              
            }

            return returnHash;
        }

        public async Task<IEnumerable<Context>> GetContextForSession(string userName, string sessionID)
        {
            // use HttpClient to call for GET method to check if session exists
            var response = await _httpClient.GetAsync($"/api/Context/{userName}/{sessionID}");
            // ensure recieved successfull answer 
            response.EnsureSuccessStatusCode();

            // decode answer into stream and de-serialize it from JSON to bool
            using var responseStream = await response.Content.ReadAsStreamAsync();
            if (responseStream.Length > 0)
                return await JsonSerializer.DeserializeAsync<List<Context>>(responseStream);
            else
                return null;
        }
    }
}