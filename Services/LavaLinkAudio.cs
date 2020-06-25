using System;
using System.Collections.Generic;
using System.Text;
using Victoria;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot
{
    public class LavaLinkAudio
    {
        private readonly LavaNode _lavaNode;

        public LavaLinkAudio(LavaNode lavaNode)
            => _lavaNode = lavaNode;

        public async Task JoinAsync( IVoiceState voiceState, ITextChannel textChannel)
        {
            await _lavaNode.JoinAsync(voiceState.VoiceChannel, textChannel);
        }

        public async Task LeaveAsync(IGuild guild)
        {
            await _lavaNode.LeaveAsync(_lavaNode.GetPlayer(guild).VoiceChannel);
        }
    }
}
