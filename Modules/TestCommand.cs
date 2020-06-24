using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class TestCommand : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        [Summary("hello commands")]
        public async Task HelloAsync(string hello)
        {
            var userinfo = Context.User.Username;
            await ReplyAsync($"Hello whits {userinfo} to {hello}");
        }
    }
}
