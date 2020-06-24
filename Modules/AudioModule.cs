using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Victoria;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class AudioModule:ModuleBase<SocketCommandContext>
    {
        public LavaLinkAudio AudioService { get; set; }

        [Command("Join")]
        public async Task JoinAndPlay()
            =>await AudioService.JoinAsync(Context.User as IVoiceState, Context.Channel as ITextChannel);

    }
}

