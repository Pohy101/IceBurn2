using System;
using Console = System.Console;
using System.Collections.Generic;
using MelonLoader;
using IceBurn.Other;
using System.Diagnostics;
using UnityEngine;
using Il2CppSystem.Threading;
using IceBurn.Mod.Other;
using IceBurn.Mod.InputHandler;
using System.IO;
using System.Net;
using IceBurn.Mod.Social;

namespace IceBurn
{
    public class Main : MelonMod
    {
        public static List<VRmod> Addons = new List<VRmod>();

        public override void OnApplicationStart()
        {
            Console.Title = "Ice Burn 2 - BY IceFox";
            Application.targetFrameRate = 144;

            Addons.Add(new InputHandler());
            Addons.Add(new Mod.UI());
            Addons.Add(new Discord());
            Addons.Add(new FOVChanger());
            Addons.Add(new SocialButtons());

            InitDiscordRP();
            Mod.Bones.GDB.OnApplicationStartX();
        }

        public override void OnUpdate()
        {
            foreach (var item in Addons)
                item.OnUpdate();

            Mod.Bones.GDB.OnUpdateX();
        }

        public override void OnGUI()
        {
            foreach (var item in Addons)
                item.OnGUI();
        }

        public override void OnFixedUpdate()
        {
            foreach (var item in Addons)
                item.OnFixedUpdate();
        }

        public override void OnLateUpdate()
        {
            foreach (var item in Addons)
                item.OnLateUpdate();
        }

        public override void VRChat_OnUiManagerInit()
        {
            foreach (var item in Addons)
                item.OnStart();
        }

        public override void OnApplicationQuit()
        {
            foreach (var item in Addons)
                item.OnQuit();
            Thread.Sleep(500);
            Process.GetCurrentProcess().Kill();
        }

        private static void InitDiscordRP()
        {
            ModPrefs.RegisterPrefBool("vrcdiscordpresence", "hidenameondiscord", true, "Hide your name on Discord", false);
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\Dependencies")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\Dependencies"));
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\Dependencies\\discord-rpc.dll")))
                using (WebClient webClient = new WebClient())
                    webClient.DownloadFile("http://thetrueyoshifan.com/downloads/discord-rpc.dll", Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\Dependencies\\discord-rpc.dll"));
        }
    }
}
