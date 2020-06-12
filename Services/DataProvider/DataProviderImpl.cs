using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using sm_coding_challenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace sm_coding_challenge.Services.DataProvider
{
    public class DataProviderImpl : IDataProvider
    {
        public static TimeSpan Timeout = TimeSpan.FromSeconds(30);
        public static string BaseURL = "https://gist.githubusercontent.com/RichardD012/a81e0d1730555bc0d8856d1be980c803/raw/3fe73fafadf7e5b699f056e55396282ff45a124b/basic.json";

        /**
        * <summary>asynsc/wait get the response data from  Feed Providers API </summary>
        */
        public async Task<DataResponseModel> GetResponseDataAsync()
        {
            var dataResponse = new DataResponseModel();
            {
                var handler = new HttpClientHandler()

                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = Timeout;
                    HttpResponseMessage response = await client.GetAsync(BaseURL);
                    var stringData = response.Content.ReadAsStringAsync().Result;
                    int statusCode = (int)response.StatusCode;
                    //Check for Http status code
                    if (statusCode == 200)
                    {

                        dataResponse = JsonConvert.DeserializeObject<DataResponseModel>(stringData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    }
                    else
                    {
                        dataResponse = null;
                    }
                }

                return dataResponse;
            }
        }

        /**
        * <summary>First instance of player  </summary>
        */

        public PlayerModel GetPlayerById(string id)
        {
            var dataResponse = GetResponseDataAsync().Result;

            //Check to see if node exists - fail gracefully 
            if (dataResponse.Rushing != null)
            {
                foreach (var player in dataResponse.Rushing)
                {
                    if (player.Id.Equals(id))
                    {
                        return player;
                    }
                }
            }
            if (dataResponse.Passing != null)
            {
                foreach (var player in dataResponse.Passing)
                {
                    if (player.Id.Equals(id))
                    {
                        return player;
                    }
                }
            }
            if (dataResponse.Receiving != null)
            {
                foreach (var player in dataResponse.Receiving)
                {
                    if (player.Id.Equals(id))
                    {
                        return player;
                    }
                }
            }
            if (dataResponse.Kicking != null)
            {
                foreach (var player in dataResponse.Kicking)
                {
                    if (player.Id.Equals(id))
                    {
                        return player;
                    }

                }
            }

            return null;
        }


        /**
        * <summary>List of first instance of players - Distinct list even if duplicate ids are passed </summary>
        */

        public List<PlayerModel> GetPlayersById(string[] idList)
        {
            var players = new List<PlayerModel>();
            var dataResponse = GetResponseDataAsync().Result;

            //Use set to store player ids
            var set = new HashSet<string>();

            foreach (var id in idList)
            {
                if (dataResponse.Rushing != null)
                {
                    foreach (var player in dataResponse.Rushing)
                    {
                        if (player.Id.Equals(id) && (!set.Contains(player.Id)))
                        {
                            players.Add(player);
                            set.Add(player.Id);
                        }
                    }
                }
                if (dataResponse.Passing != null)
                {
                    foreach (var player in dataResponse.Passing)
                    {
                        if (player.Id.Equals(id) && (!set.Contains(player.Id)))
                        {
                            players.Add(player);
                            set.Add(player.Id);
                        }
                    }
                }
                if (dataResponse.Receiving != null)
                {
                    foreach (var player in dataResponse.Receiving)
                    {
                        if (player.Id.Equals(id) && (!set.Contains(player.Id)))
                        {
                            players.Add(player);
                            set.Add(player.Id);

                        }
                    }
                }
                if (dataResponse.Kicking != null)
                {
                    foreach (var player in dataResponse.Kicking)
                    {
                        if (player.Id.Equals(id) && (!set.Contains(player.Id)))
                        {
                            players.Add(player);
                            set.Add(player.Id);

                        }
                    }
                }
            }
            return players;
        }

        /**
        * <summary>List  all instances of player  - returns multiple instances if exists</summary>
         */

        public List<PlayerModel> GetLatestPlayers(string[] idList)
        {
            var players = new List<PlayerModel>();
            var dataResponse = GetResponseDataAsync().Result;
            foreach (var id in idList)
            {

                if (dataResponse.Rushing != null)
                {
                    foreach (var player in dataResponse.Rushing)
                    {
                        if (player.Id.Equals(id))
                        {
                            players.Add(player);
                        }
                    }
                }
                if (dataResponse.Passing != null)
                {
                    foreach (var player in dataResponse.Passing)
                    {
                        if (player.Id.Equals(id))
                        {
                            players.Add(player);
                        }
                    }
                }

                if (dataResponse.Receiving != null)
                {
                    foreach (var player in dataResponse.Receiving)
                    {
                        if (player.Id.Equals(id))
                        {
                            players.Add(player);
                        }
                    }
                }
                if (dataResponse.Kicking != null)
                {
                    foreach (var player in dataResponse.Kicking)
                    {
                        if (player.Id.Equals(id))
                        {
                            players.Add(player);
                        }
                    }
                }

            }

            return players;
        }

        /*
         *Method to return distinct list of all players -if  player not  found in first node(Rushing) proceed to second 
         * node(Passing)and so on ..
         *
         */
        public List<PlayerModel> GetPlayers()
        {
            var players = new List<PlayerModel>();
            //Use set to store player ids - we need distinct list of players
            var set = new HashSet<string>();

            var dataResponse = GetResponseDataAsync().Result;

            if (dataResponse.Kicking != null)
            {
                foreach (var player in dataResponse.Rushing)
                    if (!set.Contains(player.Id))
                    {
                        set.Add(player.Id);
                        players.Add(player);
                    }
            }
            if (dataResponse.Kicking != null)
            {
                foreach (var player in dataResponse.Passing)
                    if (!set.Contains(player.Id))
                    {
                        players.Add(player);
                        set.Add(player.Id);
                    }
            }
            if (dataResponse.Kicking != null)
            {
                foreach (var player in dataResponse.Receiving)
                    if (!set.Contains(player.Id))
                    {
                        players.Add(player);
                        set.Add(player.Id);
                    }
            }
            if (dataResponse.Kicking != null)
            {
                foreach (var player in dataResponse.Kicking)
                    if (!set.Contains(player.Id))
                    {
                        players.Add(player);
                        set.Add(player.Id);
                    }

            }
            return players;
        }
    }
}