using IceBurn.Other;
using DiscordRPC;
using System;
using System.Text;
using System.Threading;

namespace IceBurn.Other
{
    class Discord : VRmod
    {
        public override string Name => "Discord Rich Presence";
        public override string Description => "Status for discord";

        private static int discordPipe = -1;
        private static RichPresence presence = new RichPresence()
        {
            Details = "Testing IceBurn 2.0",
            State = "Playing VRChat",
            Timestamps = Timestamps.FromTimeSpan(10),
            Assets = new Assets()
            {
                LargeImageKey = "large",
                LargeImageText = "C#",
                SmallImageKey = "smallVR",
                SmallImageText = "VRChat"
            }
        };

        public override void OnStart()
        {
            var client = new DiscordRpcClient("702183174205210674", pipe: discordPipe) { };

            client.OnReady += (sender, msg) =>
            {
                Console.WriteLine("Connected to discord with user {0}", msg.User.Username);
            };

            client.OnPresenceUpdate += (sender, msg) =>
            {
                Console.WriteLine("Presence has been updated! ");
            };

            client.Initialize();
            client.SetPresence(presence);
            //Console.ReadKey();
            client.Dispose();
        }
    }
}
