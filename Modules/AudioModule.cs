using System;
using Victoria;
using Victoria.Enums;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using Victoria.EventArgs;

namespace DiscordBot
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly LavaNode _lavaNode;
        public AudioModule(LavaNode lavaNode)
        => _lavaNode = lavaNode;

        [Command("Join")]
        public async Task Join()
        {
            Embed embed;

            if (_lavaNode.HasPlayer(Context.Guild))
            {
                embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription($"👌-I already connected to {(Context.User as IVoiceState).VoiceChannel.Name} 👽")
                .WithColor(Color.DarkRed)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

                await ReplyAsync(embed: embed);
                return;
            }

            if((Context.User as IVoiceState).VoiceChannel == null)
            {
                embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription("You IDIOT???\n You not connected to voice chennel!!!")
                .WithColor(Color.DarkRed)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

                await ReplyAsync(embed: embed);
                return;
            }

            await _lavaNode.JoinAsync((Context.User as IVoiceState).VoiceChannel, Context.Channel as ITextChannel);
            embed = new EmbedBuilder()
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
            var player = _lavaNode.GetPlayer(Context.Guild);

            if (player.PlayerState == PlayerState.Playing)
            {
                await player.StopAsync();
            }

            player = null;

            await _lavaNode.LeaveAsync((Context.User as IVoiceState).VoiceChannel);

            var embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription($"OK i'm leaving now  😢")
                .WithColor(Color.DarkPurple)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("Play")]
        public async Task Play([Remainder]string url)
        {
            Embed embed;
            if ((Context.User as IVoiceState).VoiceChannel == null)
            {
                embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription("You IDIOT???\n You not connected to voice chennel!!!")
                .WithColor(Color.DarkRed)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

                await ReplyAsync(embed: embed);
                return;
            }
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await _lavaNode.JoinAsync((Context.User as IVoiceState).VoiceChannel, Context.Channel as ITextChannel);
            }

            var player = _lavaNode.GetPlayer(Context.Guild);
            LavaTrack track;
            var search = await _lavaNode.SearchAsync(url);

            if(search.LoadStatus == LoadStatus.NoMatches)
            {
                embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription("😭 I did not find anything!")
                .WithColor(Color.DarkOrange)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

                await ReplyAsync(embed: embed);
                return;
            }

            track = search.Tracks.FirstOrDefault();

            if(player.Track!=null&&player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                player.Queue.Enqueue(track);
                embed = new EmbedBuilder()
                .WithTitle("🎵 Music 🎵")
                .WithDescription($"😁 I add {track.Title} to queue 👍")
                .WithColor(Color.DarkOrange)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();
                await ReplyAsync(embed: embed);
                return;
            }

            await player.PlayAsync(track);
            player.Queue.Enqueue(track);
            embed = new EmbedBuilder()
            .WithTitle("🎵 Music 🎵")
            .WithDescription($"{track.Title}")
            .WithImageUrl($"https://img.youtube.com/vi/{url.Substring(url.IndexOf('=')+1)}/maxresdefault.jpg")
            .WithColor(Color.DarkPurple)
            .WithTimestamp(DateTimeOffset.Now)
            .Build();
            await ReplyAsync(embed: embed);
        }


        [Command("Ps")]
        public async Task Pause()
        {
            var player = _lavaNode.GetPlayer(Context.Guild);

            if (player.PlayerState == PlayerState.Playing)
            {
                await player.PauseAsync();
            }

            var embed = new EmbedBuilder()
            .WithTitle("🎵 Music 🎵")
            .WithDescription($"{player.Track.Title} Paused!!!")
            .WithImageUrl($"https://img.youtube.com/vi/{player.Track.Url.Substring(player.Track.Url.IndexOf('=') + 1)}/hqdefault.jpg")
            .WithColor(Color.Gold)
            .WithTimestamp(DateTimeOffset.Now)
            .Build();
            await ReplyAsync(embed: embed);
        }

        [Command("Res")]
        public async Task Resume()
        {
            var player = _lavaNode.GetPlayer(Context.Guild);

            if (player.PlayerState == PlayerState.Paused)
            {
                await player.ResumeAsync();
            }

            var embed = new EmbedBuilder()
            .WithTitle("🎵 Music 🎵")
            .WithDescription($"{player.Track.Title} Resumed!!!")
            .WithImageUrl($"https://img.youtube.com/vi/{player.Track.Url.Substring(player.Track.Url.IndexOf('=') + 1)}/hqdefault.jpg")
            .WithColor(Color.Gold)
            .WithTimestamp(DateTimeOffset.Now)
            .Build();
            await ReplyAsync(embed: embed);
        }



        public static async Task TrackEnd(TrackEndedEventArgs args)
        {

            if (!args.Reason.ShouldPlayNext())
            {
                return;
            }

            if (!args.Player.Queue.TryDequeue(out var queueable))
            {
                return;
            }

            if (!(queueable is LavaTrack track))
            {
                await args.Player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
                return;
            }

            await args.Player.PlayAsync(track);


            //args.Player.Queue.TryDequeue(out var next);
            //LavaTrack track = next as LavaTrack;
            //await args.Player.PlayAsync(track);

            ////var embed = new EmbedBuilder()
            ////.WithTitle("🎵 Music 🎵")
            ////.WithDescription($"{track.Title}")
            ////.WithImageUrl($"https://img.youtube.com/vi/{track.Url.Substring(track.Url.IndexOf('=') + 1)}/hqdefault.jpg")
            ////.WithColor(Color.DarkPurple)
            ////.WithTimestamp(DateTimeOffset.Now)
            ////.Build();
            //// await ReplyAsync(embed: embed);
        }
    }
}
