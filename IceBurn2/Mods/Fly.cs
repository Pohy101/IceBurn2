using IceBurn.API;
using IceBurn.Mod;
using IceBurn.Utils;
using IceModSystem;
using Logger;
using System;
using UnityEngine;

namespace IceBurn.Mods
{
    public class Fly : VRmod
    {
        public override int LoadOrder => 7;

        public static QMNestedButton flyMenu;
        public static QMSingleButton resetflySpeed;
        public static QMHalfButton flySpeedUp;
        public static QMHalfButton flySpeedDown;
        public static QMHalfButton flySpeedUpX;
        public static QMHalfButton flySpeedDownX;
        public static QMToggleButton toggleFly;
        public static QMSingleButton ohShiitFly;

        public override void OnStart()
        {
            flyMenu = new QMNestedButton(UI.mainMenuP1, 1, 0, "Fly\nMenu", "Fly Menu");

            resetflySpeed = new QMSingleButton(flyMenu, 2, 0, "Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]", new Action(() =>
            {
                GlobalUtils.flySpeed = 5;
                resetflySpeed.setButtonText("Reset\nSpeed\n[5]");
            }), "Reset Fly Speed To Default");

            flySpeedUp = new QMHalfButton(flyMenu, 3, -0.5f, "▲", new Action(() =>
            {
                GlobalUtils.flySpeed++;
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
            }), "Fly Speed Up");

            flySpeedDown = new QMHalfButton(flyMenu, 3, 0.5f, "▼", new Action(() =>
            {
                if (GlobalUtils.flySpeed > 0)
                    GlobalUtils.flySpeed--;
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
            }), "Fly Speed Down");

            flySpeedUpX = new QMHalfButton(flyMenu, 4, -0.5f, "▲▲▲", new Action(() =>
            {
                GlobalUtils.flySpeed += 3;
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
            }), "Fly Speed Up");

            flySpeedDownX = new QMHalfButton(flyMenu, 4, 0.5f, "▼▼▼", new Action(() =>
            {
                if (GlobalUtils.flySpeed > 0)
                    GlobalUtils.flySpeed -= 3;
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
            }), "Fly Speed Down");

            toggleFly = new QMToggleButton(flyMenu, 1, 0,
            "Fly ON", new Action(() =>
            {
                GlobalUtils.Fly = true;
                PlayerWrapper.GetCurrentPlayer().GetComponent<CharacterController>().enabled = false;

                flySpeedUp.setActive(true);
                flySpeedDown.setActive(true);
                flySpeedUpX.setActive(true);
                flySpeedDownX.setActive(true);
                resetflySpeed.setActive(true);
                ohShiitFly.setActive(true);

                IceLogger.Log("Fly has been Enabled");
            }), "Fly OFF", new Action(() =>
            {
                GlobalUtils.Fly = false;
                PlayerWrapper.GetCurrentPlayer().GetComponent<CharacterController>().enabled = true;

                flySpeedUp.setActive(false);
                flySpeedDown.setActive(false);
                flySpeedUpX.setActive(false);
                flySpeedDownX.setActive(false);
                resetflySpeed.setActive(false);
                ohShiitFly.setActive(false);

                IceLogger.Log("Fly has been Disabled");
            }), "Toggle Fly");

            ohShiitFly = new QMSingleButton(flyMenu, 1, 2, "SHEEET", new Action(() =>
            {
                GlobalUtils.flySpeed += 1000;
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
            }), "OH SHEEEEEEEEEEEEEEEEEEEET");

            toggleFly.setToggleState(false);
            flySpeedUp.setActive(false);
            flySpeedDown.setActive(false);
            resetflySpeed.setActive(false);
            flySpeedUpX.setActive(false);
            flySpeedDownX.setActive(false);
            ohShiitFly.setActive(false);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GlobalUtils.Fly = !GlobalUtils.Fly;
                toggleFly.setToggleState(GlobalUtils.Fly);
                if (GlobalUtils.Fly)
                {
                    PlayerWrapper.GetCurrentPlayer().GetComponent<CharacterController>().enabled = false;

                    flySpeedUp.setActive(true);
                    flySpeedDown.setActive(true);
                    resetflySpeed.setActive(true);
                    flySpeedUpX.setActive(true);
                    flySpeedDownX.setActive(true);
                    ohShiitFly.setActive(true);

                    IceLogger.Log("Fly has been Enabled");
                }
                else
                {
                    PlayerWrapper.GetCurrentPlayer().GetComponent<CharacterController>().enabled = true;

                    flySpeedUp.setActive(false);
                    flySpeedDown.setActive(false);
                    resetflySpeed.setActive(false);
                    flySpeedUpX.setActive(false);
                    flySpeedDownX.setActive(false);
                    ohShiitFly.setActive(false);

                    IceLogger.Log("Fly has been Disabled");
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                GlobalUtils.flySpeed *= 2;
                GlobalUtils.walkSpeed *= 2;
                GlobalUtils.UpdatePlayerSpeed();
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                GlobalUtils.flySpeed /= 2;
                GlobalUtils.walkSpeed /= 2;
                GlobalUtils.UpdatePlayerSpeed();
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }

            if (GlobalUtils.flySpeed <= 0)
            {
                GlobalUtils.flySpeed = 1;
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }

            if (GlobalUtils.Fly)
            {
                GameObject player = PlayerWrapper.GetCurrentPlayer().gameObject;
                GameObject playercamera = Wrapper.GetPlayerCamera();

                /*if (GlobalUtils.flySpeed <= 0)
                {
                    GlobalUtils.flySpeed = 1;
                    UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                }*/

                if (Input.mouseScrollDelta.y != 0)
                {
                    GlobalUtils.flySpeed += (int)Input.mouseScrollDelta.y;
                    resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                }

                if (Input.GetKey(KeyCode.W))
                    player.transform.position += playercamera.transform.forward * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.A))
                    player.transform.position -= playercamera.transform.right * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.S))
                    player.transform.position -= playercamera.transform.forward * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.D))
                    player.transform.position += playercamera.transform.right * GlobalUtils.flySpeed * Time.deltaTime;

                if (Input.GetKey(KeyCode.E))
                    player.transform.position += playercamera.transform.up * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.Q))
                    player.transform.position -= playercamera.transform.up * GlobalUtils.flySpeed * Time.deltaTime;

                if (System.Math.Abs(Input.GetAxis("Joy1 Axis 2")) > 0f)
                    player.transform.position += playercamera.transform.forward * GlobalUtils.flySpeed * Time.deltaTime * (Input.GetAxis("Joy1 Axis 2") * -1f);
                if (System.Math.Abs(Input.GetAxis("Joy1 Axis 1")) > 0f)
                    player.transform.position += playercamera.transform.right * GlobalUtils.flySpeed * Time.deltaTime * Input.GetAxis("Joy1 Axis 1");
            }
        }
    }
}
