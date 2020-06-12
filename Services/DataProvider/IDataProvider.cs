using sm_coding_challenge.Models;

using System.Collections.Generic;

namespace sm_coding_challenge.Services.DataProvider
{
    public interface IDataProvider
    {
        PlayerModel GetPlayerById(string id);
        //Gets  distinct list of Players- selects player from first matched node 
        List<PlayerModel> GetPlayers();
        //Get the first instance of players - returns distinct list even if duplicate ids are passed
        List<PlayerModel> GetPlayersById(string[] idList);
        //Gets all occurances of players in all nodes (Rushing,Passing,Kicking,Receiving)
        List<PlayerModel> GetLatestPlayers(string[] idList);
        
    }
}
