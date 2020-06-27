using IceBurn.Other;
using DiscordRichPresence;

namespace IceBurn.Mod.Other
{
    class Discord : VRmod
    {
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
