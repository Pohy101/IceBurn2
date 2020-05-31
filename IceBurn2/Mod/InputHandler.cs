using IceBurn.Other;
using IceBurn.Utils;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.SDKBase;
using VRC;
using VRC.Core;
using VRC.UI;
using System.Threading;

namespace IceBurn.Mod
{
    class InputHandler : VRmod
    {
        public override string Name => "Inputs";
        public override string Description => "Inputs handling from here";

        // Инициализация
        public InputHandler() : base()
        {

        }

        public override void OnStart()
        {

        }

        public override void OnUpdate()
        {
            // Включает или выключает полёт при нажатий на F
            if (Input.GetKeyDown(KeyCode.F))
            {
                GlobalUtils.Fly = !GlobalUtils.Fly;
                UI.toggleFly.setToggleState(GlobalUtils.Fly);
                if (GlobalUtils.Fly)
                {
                    Physics.gravity = Vector3.zero;
                    GlobalUtils.ToggleColliders(false);

                    UI.flySpeedUp.setActive(true);
                    UI.flySpeedDown.setActive(true);
                    UI.resetflySpeed.setActive(true);
                    UI.flySpeedUpX.setActive(true);
                    UI.flySpeedDownX.setActive(true);
                    UI.ohShiitFly.setActive(true);

                    IceLogger.Log("Fly has been Enabled");
                }
                else
                {
                    Physics.gravity = GlobalUtils.Gravity;
                    GlobalUtils.ToggleColliders(true);

                    UI.flySpeedUp.setActive(false);
                    UI.flySpeedDown.setActive(false);
                    UI.resetflySpeed.setActive(false);
                    UI.flySpeedUpX.setActive(false);
                    UI.flySpeedDownX.setActive(false);
                    UI.ohShiitFly.setActive(false);

                    IceLogger.Log("Fly has been Disabled");
                }
            }

            // Телепорт в точку которая находится на центре экрана по кнопку T
            if (Input.GetKeyDown(KeyCode.T))
                GlobalUtils.RayTeleport();

            // Клонирование аватара на кнопку B [при выбранном игроке]
            if (Input.GetKeyDown(KeyCode.B))
            {
                try
                {
                    ApiAvatar avatar = Wrapper.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0;

                    if (avatar.releaseStatus != "private")
                        new PageAvatar { avatar = new SimpleAvatarPedestal { field_Internal_ApiAvatar_0 = new ApiAvatar { id = avatar.id } } }.ChangeToSelectedAvatar();
                    else
                    {
                        IceLogger.Log("Avatar release status is PRIVATE!");
                    }
                }
                catch (Exception)
                {
                    IceLogger.Error("User not selected!");
                    throw;
                }
            }

            // ESP или видеть игроков сквазь стены на кнопку O
            if (Input.GetKeyDown(KeyCode.O))
            {
                GlobalUtils.ESP = !GlobalUtils.ESP;
                UI.toggleESP.setToggleState(GlobalUtils.ESP);

                // Поиск и добавление игроков в ESP
                GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < array.Length; i++)
                {
                    Transform sRegion = array[i].transform.Find("SelectRegion");
                    if (sRegion != null)
                    {
                        sRegion.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(sRegion.GetComponent<Renderer>(), GlobalUtils.ESP);
                    }
                }

                // Поиск и добавление обьектов в ESP И СУКА ПОЧЕМУ ТО ОНО НЕ РАБОТАЕТ!
                /*foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);
                        Wrapper.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.ESP);
                    }
                }*/
            }

            // Ебать уши другим игрокам на F9
            if (Input.GetKeyDown(KeyCode.F9))
            {
                if (USpeaker.field_Internal_Static_Single_1 <= 1f)
                {
                    UI.toggleEarRape.setToggleState(true);
                    USpeaker.field_Internal_Static_Single_1 = float.MaxValue;
                    IceLogger.Log("EarRape Enabled");
                }
                else
                {
                    UI.toggleEarRape.setToggleState(false);
                    USpeaker.field_Internal_Static_Single_1 = 1f;
                    IceLogger.Log("EarRape Disabled");
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                GlobalUtils.flySpeed *= 2;
                GlobalUtils.walkSpeed *= 2;
                GlobalUtils.UpdatePlayerSpeed();
                UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                GlobalUtils.flySpeed /= 2;
                GlobalUtils.walkSpeed /= 2;
                GlobalUtils.UpdatePlayerSpeed();
                UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.X) && Input.mouseScrollDelta.y != 0)
            {
                GlobalUtils.walkSpeed += (int)Input.mouseScrollDelta.y;
                GlobalUtils.UpdatePlayerSpeed();
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                IceLogger.Log(Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_USpeaker_0.SpeakerVolume.ToString());
                /*Networking.GoToRoom(Wrapper.GetInstance().instanceWorld.id + ":" + Wrapper.GetInstance().instanceWorld.currentInstanceIdWithTags);
                IceLogger.Log(Wrapper.GetInstance().instanceWorld.id + ":" + Wrapper.GetInstance().instanceWorld.currentInstanceIdWithTags);*/
            }

            if (GlobalUtils.walkSpeed <= 0)
            {
                GlobalUtils.walkSpeed = 1;
                GlobalUtils.UpdatePlayerSpeed();
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }

            if (GlobalUtils.ESP)
            {
                HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", Color.red);
            }

            // Управление во время полёта
            if (GlobalUtils.Fly)
            {
                GameObject playercamera = Wrapper.GetPlayerCamera();
                VRCPlayer player = PlayerWrapper.GetCurrentPlayer();

                if (GlobalUtils.flySpeed <= 0)
                {
                    GlobalUtils.flySpeed = 1;
                    UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                }

                if (Input.mouseScrollDelta.y != 0)
                {
                    GlobalUtils.flySpeed += (int)Input.mouseScrollDelta.y;
                    UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                }

                if (Input.GetKey(KeyCode.W))
                    player.transform.position += playercamera.transform.forward * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.S))
                    player.transform.position -= playercamera.transform.forward * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.A))
                    player.transform.position -= playercamera.transform.right * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.D))
                    player.transform.position += playercamera.transform.right * GlobalUtils.flySpeed * Time.deltaTime;

                if (Input.GetKey(KeyCode.E))
                    player.transform.position += playercamera.transform.up * GlobalUtils.flySpeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.Q))
                    player.transform.position -= playercamera.transform.up * GlobalUtils.flySpeed * Time.deltaTime;

                if (Math.Abs(Input.GetAxis("Joy1 Axis 2")) > 0f)
                    player.transform.position += playercamera.transform.forward * GlobalUtils.flySpeed * Time.deltaTime * (Input.GetAxis("Joy1 Axis 2") * -1f);
                if (Math.Abs(Input.GetAxis("Joy1 Axis 1")) > 0f)
                    player.transform.position += playercamera.transform.right * GlobalUtils.flySpeed * Time.deltaTime * Input.GetAxis("Joy1 Axis 1");
            }
        }
        public override void OnFixedUpdate()
        {

        }
    }
}
