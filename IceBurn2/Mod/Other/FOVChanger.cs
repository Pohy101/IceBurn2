using IceBurn.Other;
using IceBurn.Utils;
using UnityEngine;

namespace IceBurn.Mod.Other
{
    class FOVChanger : VRmod
    {
        public static bool isPC = false;

        public override void OnStart()
        {
            Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
        }

        public override void OnUpdate()
        {
            if (isPC)
            {
                if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.LeftAlt) && Input.mouseScrollDelta.y != 0f)
                {
                    GlobalUtils.cameraFOV -= (int)Input.mouseScrollDelta.y;
                    Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                    UI.resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
                }

                if (GlobalUtils.cameraFOV > 100)
                {
                    GlobalUtils.cameraFOV = 100;
                    Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                    UI.resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
                }
                if (GlobalUtils.cameraFOV < 0f)
                {
                    GlobalUtils.cameraFOV = 0;
                    Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                    UI.resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
                }
            }
        }
    }
}