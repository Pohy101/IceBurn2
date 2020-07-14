using DiscordRPC;
using IceModSystem;
using Logger;
using System;

namespace IceBurn.Mod
{
    public class Discord : VRmod
    {
        public override int LoadOrder => 5;

        DiscordRpcClient client;
        RichPresence presence;
        RichPresence VRpresence = new RichPresence()
        {
            Details = "Logged in [VR]",
            State = "Loading world...",
            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },
            Assets = new Assets()
            {
                LargeImageKey = "logo",
                SmallImageKey = "VR",
                LargeImageText = "VRChat2",
                SmallImageText = "VR"
            }
        };

        RichPresence DesktopPresence = new RichPresence()
        {
            Details = "Logged in [Desktop]",
            State = "Loading world...",
            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },
            Assets = new Assets()
            {
                LargeImageKey = "logo",
                SmallImageKey = "Desktop",
                LargeImageText = "VRChat2",
                SmallImageText = "Desktop"
            }
        };

        private void initClient()
        {
            client = new DiscordRpcClient("711923739188527124");
            if (client.Initialize())
            {
                if (VRCTrackingManager.Method_Public_Static_Boolean_11())
                    presence = VRpresence;
                else
                    presence = DesktopPresence;
                client.SetPresence(presence);
                IceLogger.Log("DiscordRPC Initialised!");
            }
            else
                IceLogger.Error("DiscordRPC Initialise Error!");
        }

        public override void OnStart()
        {
            initClient();
        }

        public override void OnUpdate()
        {

        }

        public override void OnQuit()
        {
            client.Dispose();
        }
    }
}
