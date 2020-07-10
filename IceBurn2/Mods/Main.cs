using IceModSystem;
using MelonLoader;
using System;
using System.IO;
using UnityEngine;
using Console = System.Console;

namespace IceBurn
{
    public class Main : VRmod
    {
        public override int LoadOrder => 0;

        public override void OnEarlierStart()
        {
            Console.Title = "Ice Burn 2 - BY IceFox";
            Application.targetFrameRate = 144;
            ModPrefs.RegisterPrefBool("vrcdiscordpresence", "hidenameondiscord", true, "Hide your name on Discord", false);

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\Dependencies")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\Dependencies"));

            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\AvatarFav")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\AvatarFav"));
        }
    }
}
