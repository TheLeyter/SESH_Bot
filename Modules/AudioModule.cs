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
            => await AudioService.JoinAsync( Context.User as IVoiceState, Context.Channel as ITextChannel);
    }
}
