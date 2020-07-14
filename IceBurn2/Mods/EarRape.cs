using IceBurn.API;
using IceModSystem;
using Logger;
using System;
using UnityEngine;

namespace IceBurn.Mod
{
    public class EarRape : VRmod
    {
        public override int LoadOrder => 13;

        public static QMToggleButton toggleEarRape;

        public override void OnStart()
        {
            toggleEarRape = new QMToggleButton(UI.mainMenuP1, 4, 0,
            "EarRape ON", new Action(() =>
            {
                USpeaker.field_Internal_Static_Single_1 = float.MaxValue;
                IceLogger.Log(ConsoleColor.Red, "EarRape Enabled");
            }), "EarRape OFF", new Action(() =>
            {
                USpeaker.field_Internal_Static_Single_1 = 1f;
                IceLogger.Log(ConsoleColor.Green, "EarRape Disabled");
            }), "Toggle EarRape");
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F9))
                if (USpeaker.field_Internal_Static_Single_1 <= 1f)
                {
                    toggleEarRape.setToggleState(true);
                    USpeaker.field_Internal_Static_Single_1 = float.MaxValue;
                    IceLogger.Log("EarRape Enabled");
                }
                else
                {
                    toggleEarRape.setToggleState(false);
                    USpeaker.field_Internal_Static_Single_1 = 1f;
                    IceLogger.Log("EarRape Disabled");
                }
        }
    }
}
