using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Victoria;
namespace DiscordBot
{
    public class DiscordService
    {
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        private ServiceProvider _services;
        private LavaNode _lavaNode;
        private LavaLinkAudio _audioService;
        private Configuration _config;

        public DiscordService()
        {
            _services = ConfigurServices();
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _commandHandler = _services.GetRequiredService<CommandHandler>();
            _lavaNode = _services.GetRequiredService<LavaNode>();
            _audioService = _services.GetRequiredService<LavaLinkAudio>();

            _client.Ready += ReadyAsync;
            _client.Log += LogClientAsync;
            _lavaNode.OnLog += LogLavaAsync;

        }

        public async Task InitializeAsync()
        {
            _config = new Configuration();
            await _client.LoginAsync(TokenType.Bot, _config.conf.Token);
            await _client.StartAsync();
            await _commandHandler.InitializeAsync();
            await Task.Delay(-1);
        }

        public async Task ReadyAsync()
        {
            if (!_lavaNode.IsConnected)
                await _lavaNode.ConnectAsync();
            await _client.SetGameAsync(_config.conf.GameStatus);
        }
        private Task LogClientAsync(LogMessage logMessage)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(logMessage.ToString());
            Console.ResetColor();
            return Task.CompletedTask;
        }
        private Task LogLavaAsync(LogMessage logMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(logMessage.ToString());
            Console.ResetColor();
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigurServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<LavaNode>()
                .AddSingleton(new LavaConfig())
                .AddSingleton<LavaLinkAudio>()
                .BuildServiceProvider();
        }
    }
}
