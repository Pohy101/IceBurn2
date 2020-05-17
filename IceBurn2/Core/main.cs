using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using IceBurn.Other;
using IceBurn.Mod;
using Org.BouncyCastle.Asn1.X509;
using System.Net;

namespace IceBurn
{
    public class main : MelonMod
    {
        public static List<VRmod> Addons = new List<VRmod>();

        public override void OnApplicationStart()
        {
            Addons.Add(new InputHandler());
            Addons.Add(new GUI());
            Addons.Add(new Discord());
        }

        public override void OnUpdate()
        {
            foreach (var item in Addons)
                item.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            foreach (var item in Addons)
                item.OnFixedUpdate();
        }

        public override void VRChat_OnUiManagerInit()
        {
            foreach (var item in Addons)
                item.OnStart();
        }
    }

    public class IceLogger
    {
        public static void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond);
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
            Console.Write(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond);
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
