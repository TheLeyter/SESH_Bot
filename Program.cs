using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        private static Task Main() 
            => new DiscordService().InitializeAsync();
    }
}
