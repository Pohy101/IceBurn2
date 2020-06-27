using IceBurn.Other;
using IceBurn.Utils;
using Console = System.Console;
using UnityEngine;
using VRC.SDKBase;
using VRC;
using VRC.Core;
using VRC.UI;
using Logger;
using System.Linq;

namespace IceBurn.Mod.InputHandler
{
    class InputHandler : VRmod
    {
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
                    PlayerWrapper.GetCurrentPlayer().GetComponent<CharacterController>().enabled = false;

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
                    PlayerWrapper.GetCurrentPlayer().GetComponent<CharacterController>().enabled = true;

                    UI.flySpeedUp.setActive(false);
                    UI.flySpeedDown.setActive(false);
                    UI.resetflySpeed.setActive(false);
                    UI.flySpeedUpX.setActive(false);
                    UI.flySpeedDownX.setActive(false);
                    UI.ohShiitFly.setActive(false);

                    IceLogger.Log("Fly has been Disabled");
                }
            }

            // Телепорт в точку которая находится на центре экрана по кнопку T || Shift + Mouse0
            if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0)) || Input.GetKeyDown(KeyCode.T))
                GlobalUtils.RayTeleport();

            // Клонирование аватара на кнопку B [при выбранном игроке]
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (Wrapper.GetQuickMenu().GetSelectedPlayer() != null)
                {
                    ApiAvatar avatar = Wrapper.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0;
                    if (avatar.releaseStatus != "private")
                        new PageAvatar { avatar = new SimpleAvatarPedestal { field_Internal_ApiAvatar_0 = new ApiAvatar { id = avatar.id } } }.ChangeToSelectedAvatar();
                    else
                    {
                        IceLogger.Log("Avatar release status is PRIVATE!");
                        Console.Beep();
                    }
                    IceLogger.Log(avatar.id);
                }
                else
                    IceLogger.Error("User not selected!");
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
                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_HighlightColor", Color.red);
                        Wrapper.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.ESP);
                        //HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", Color.red);
                    }
                }
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

            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                (from portal in Resources.FindObjectsOfTypeAll<PortalInternal>()
                 where portal.gameObject.activeInHierarchy && !portal.gameObject.GetComponentInParent<VRC_PortalMarker>()
                 select portal).ToList<PortalInternal>().ForEach(delegate (PortalInternal p)
                 {
                     Player component = Networking.GetOwner(p.gameObject).gameObject.GetComponent<Player>();
                     UnityEngine.Object.Destroy(p.transform.root.gameObject);
                 });
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                PlayerWrapper.UpdateFriendList();

                var allPlayers = PlayerWrapper.GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    Transform sRegion = allPlayers[i].transform.Find("SelectRegion");
                    allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.green;
                    allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.white;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.white;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.black;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlateTalkSprite = allPlayers[i].field_Internal_VRCPlayer_0.namePlateSilentSprite;

                    if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Veteran user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.red);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Trusted user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.magenta);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Known user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.Lerp(Color.yellow, Color.red, 0.5f));
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "User")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.green);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "New user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(new Color(0.3f, 0.72f, 1f));
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Visitor")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.gray);

                    if (sRegion != null)
                        sRegion.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);

                    HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", Color.red);

                    if (allPlayers[i].field_Internal_VRCPlayer_0.prop_String_1 == "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223")
                    {
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.black);
                        allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.red;
                        allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.clear;
                        allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.red;
                        allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.red;
                    }
                }
            }

            if (GlobalUtils.walkSpeed <= 0)
            {
                GlobalUtils.walkSpeed = 1;
                GlobalUtils.UpdatePlayerSpeed();
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }
            if (GlobalUtils.flySpeed <= 0)
            {
                GlobalUtils.flySpeed = 1;
                UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }
            if (GlobalUtils.brightness <= 0f)
            {
                GlobalUtils.brightness = 0.1f;
                UI.resetBrightness.setButtonText("Reset\nBrightness\n[" + GlobalUtils.brightness + "]");
            }

            // Управление во время полёта
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
                    UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
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
