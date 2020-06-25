using System;
using System.Collections.Generic;
using System.Text;
using Victoria;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        public LavaLinkAudio AudioService { get; set; }

        [Command("Join")]
        public async Task Join()
        {
            await AudioService.JoinAsync(Context.User as IVoiceState, Context.Channel as ITextChannel);

            var embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription($"👌-I connected to {(Context.User as IVoiceState).VoiceChannel.Name} 👽")
                .WithColor(Color.DarkPurple)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("Leave")]
        public async Task Leave()
        {
            await AudioService.LeaveAsync(Context.Guild);

            var embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription($"OK i'm leaving now  😢")
                .WithColor(Color.DarkPurple)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}
