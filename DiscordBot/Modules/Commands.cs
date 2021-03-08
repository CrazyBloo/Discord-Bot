using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("server")]
        public async Task ServerInfo()
        {
            using (var webClient = new System.Net.WebClient())
            {
                //Download JSON string from Steam Web API
                var json = webClient.DownloadString(@"https://api.steampowered.com/IGameServersService/GetServerList/v1/?filter=\appid\PUT_YOUR_GAME_APPID\addr\PUT_YOUR_GAME_SERVER_IP&key=PUT_YOUR_STEAM_API_KEY");
                //Usually this API gets multiple servers so the JSON is nested, here we just trim off the extra parts so its all one on level
                json = json.Replace("{\"response\":{\"servers\":[", "");
                json = json.Replace("]}}", " ");
                //Now that its trimmed we parse
                var convertedJson = JObject.Parse(json);
                var players = convertedJson["players"];
                var map = convertedJson["map"];
                //this server is always on rp_southside_day, so this is how i check if the server is up, if map != southside, shit dead
                if (map.ToString() == "rp_southside_day")
                {
                    await ReplyAsync("MafiaRP is currently online! " + players.ToString() + "/100 Players online");
                } else
                {
                    await ReplyAsync("Couldn't reach the server!" + map.ToString());
                }
                
            }
        }
    }
}
