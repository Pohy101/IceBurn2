using IceBurn.Other;
using System;
using System.IO;
using System.Net;
using MelonLoader;
using DiscordRichPresence;

namespace IceBurn.Other
{
    class otherrrrr : MelonMod
    {
        public override void OnApplicationStart()
        {
            ModPrefs.RegisterPrefBool("vrcdiscordpresence", "hidenameondiscord", true, "Hide your name on Discord", false);
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "Dependencies")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Dependencies"));
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Dependencies/discord-rpc.dll")))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("http://thetrueyoshifan.com/downloads/discord-rpc.dll", Path.Combine(Environment.CurrentDirectory, "Dependencies/discord-rpc.dll"));
                }
            }
        }
    }


    class Discord : VRmod
    {
        public override string Name => "Discord Rich Presence";
        public override string Description => "Custom status for discord";

        public override void OnStart()
        {
            DiscordManager.Init();
        }

        public override void OnUpdate()
        {
            DiscordRichPresence.DiscordManager.Update();
        }

        public override void OnQuit()
        {
            DiscordManager.OnApplicationQuit();
        }


    }
}
