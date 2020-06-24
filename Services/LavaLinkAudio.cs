using System;
using System.Collections.Generic;
using System.Text;
using Victoria;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Victoria.EventArgs;

namespace DiscordBot
{
    public class LavaLinkAudio
    {
        private readonly LavaNode _lavaNode;

        public LavaLinkAudio(LavaNode lavaNode)
            => _lavaNode = lavaNode;

        public async Task JoinAsync(IVoiceState voiceState, ITextChannel textChannel)
        {
            await _lavaNode.JoinAsync(voiceState.VoiceChannel, textChannel);
        }

        public async Task TrackEnded(TrackEndedEventArgs args)
        {
            if (!args.Reason.ShouldPlayNext())
            {
                return;
            }

            if (!args.Player.Queue.TryDequeue(out var queueable))
            {
                //await args.Player.TextChannel.SendMessageAsync("Playback Finished.");
                return;
            }

            if (!(queueable is LavaTrack track))
            {
                await args.Player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
                return;
            }

            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync($"Now Playing [{track.Title}]({track.Url})");
        }
    }
}
