using IceBurn.Other;
using System.Collections;
using UnityEngine;

namespace IceBurn.Mod
{
    class GUI : VRmod
    {
        public override string Name => "User Interface";
        public override string Description => "To see elements on screen";

        public override void OnStart()
        {

        }

        public override void OnGUI()
        {
            /*if (GUI.B(new Rect(10, 10, 150, 100), "I am a button"))
            {
                IceLogger.Log("You clicked the button!");
            }*/
           
        }

        public override void OnUpdate()
        {

        }
    }
}
