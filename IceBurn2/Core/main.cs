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

namespace IceBurn
{
    public class main : MelonMod
    {
        public static List<VRmod> Addons = new List<VRmod>();

        public override void OnApplicationStart()
        {
            Addons.Add(new InputHandler());
            Addons.Add(new Mod.GUI());
            Addons.Add(new Mod.UI());
            Addons.Add(new Discord());
            Addons.Add(new FOVChanger());
            Addons.Add(new Mod.AvFav.UI());
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public override void OnUpdate()
        {
            foreach (var item in Addons)
                item.OnUpdate();
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
            Application.targetFrameRate = 144;
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
    }

    public class IceLogger
    {
        public static void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("IceBurn");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("IceBurn");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
