using IceBurn.API;
using IceBurn.Utils;
using IceModSystem;
using System;
using UnityEngine;

namespace IceBurn.Mod
{
    class FOVChanger : VRmod
    {
        public override int LoadOrder => 7;

        public static QMHalfButton FOVUp;
        public static QMHalfButton FOVUpX;
        public static QMHalfButton FOVDown;
        public static QMHalfButton FOVDownX;
        public static QMSingleButton resetFOV;
        public static QMNestedButton FOVChangerMenu;


        public override void OnStart()
        {
            FOVChangerMenu = new QMNestedButton(UI.mainMenuP1, 1, 1, "FOV\nMenu", "Field Of View Menu");

            FOVUp = new QMHalfButton(FOVChangerMenu, 2, -0.5f, "▲", new Action(() =>
            {
                if (GlobalUtils.cameraFOV < 100)
                    GlobalUtils.cameraFOV++;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }), "Fly Speed Up");

            FOVDown = new QMHalfButton(FOVChangerMenu, 2, 0.5f, "▼", new Action(() =>
            {
                if (GlobalUtils.cameraFOV > 0)
                    GlobalUtils.cameraFOV--;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }), "Fly Speed Down");

            FOVUpX = new QMHalfButton(FOVChangerMenu, 3, -0.5f, "▲▲▲", new Action(() =>
            {
                if (GlobalUtils.cameraFOV < 100)
                    GlobalUtils.cameraFOV += 3;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }), "Fly Speed Up 3X");

            FOVDownX = new QMHalfButton(FOVChangerMenu, 3, 0.5f, "▼▼▼", new Action(() =>
            {
                if (GlobalUtils.cameraFOV > 0)
                    GlobalUtils.cameraFOV -= 3;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }), "Fly Speed Down 3X");

            resetFOV = new QMSingleButton(FOVChangerMenu, 1, 0, "Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]", new Action(() =>
            {
                GlobalUtils.cameraFOV = 60;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }), "Reset To Default Field Of View [60]");

            Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
        }

        public override void OnUpdate()
        {
            if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.LeftAlt) && Input.mouseScrollDelta.y != 0f)
            {
                GlobalUtils.cameraFOV -= (int)Input.mouseScrollDelta.y;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }

            if (GlobalUtils.cameraFOV > 100)
            {
                GlobalUtils.cameraFOV = 100;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }
            if (GlobalUtils.cameraFOV < 0f)
            {
                GlobalUtils.cameraFOV = 0;
                Wrapper.GetPlayerCamera().GetComponent<Camera>().fieldOfView = GlobalUtils.cameraFOV;
                resetFOV.setButtonText("Reset\nFOV\n[" + GlobalUtils.cameraFOV + "]");
            }
        }
    }
}