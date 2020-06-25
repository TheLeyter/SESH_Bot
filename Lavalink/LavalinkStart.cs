using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DiscordBot
{
    public class LavalinkStart
    {
        
        public static void Start()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "powershell";
            info.Arguments = $"java -jar {Environment.CurrentDirectory}"+@".\Lavalink.jar";
            Process.Start(info);
        }
    }
}
