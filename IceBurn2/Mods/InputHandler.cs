using IceBurn.Utils;
using IceBurn.Utils.Popup;
using IceModSystem;
using Logger;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;
using Console = System.Console;

namespace IceBurn.Mod
{
    class InputHandler : VRmod
    {
        public override int LoadOrder => 8;

        public override void OnUpdate()
        {
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
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                Popup.Alert("Testing", "Some text to test", "Back");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.X) && Input.mouseScrollDelta.y != 0)
            {
                GlobalUtils.walkSpeed += (int)Input.mouseScrollDelta.y;
                GlobalUtils.UpdatePlayerSpeed();
                UI.resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
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
            if (GlobalUtils.brightness <= 0f)
            {
                GlobalUtils.brightness = 0.1f;
                UI.resetBrightness.setButtonText("Reset\nBrightness\n[" + GlobalUtils.brightness + "]");
            }

            if (GlobalUtils.FreeCam)
            {
                GameObject Freecam = GameObject.Find("playerFreeCam");
                Freecam.transform.rotation = Wrapper.GetPlayerCamera().transform.rotation;

                if (Input.GetKey(KeyCode.W))
                    Freecam.transform.position += Freecam.transform.forward * (GlobalUtils.flySpeed * 0.1f);
                if (Input.GetKey(KeyCode.S))
                    Freecam.transform.position -= Freecam.transform.forward * (GlobalUtils.flySpeed * 0.1f);
                if (Input.GetKey(KeyCode.D))
                    Freecam.transform.position += Freecam.transform.right * (GlobalUtils.flySpeed * 0.1f);
                if (Input.GetKey(KeyCode.A))
                    Freecam.transform.position -= Freecam.transform.right * (GlobalUtils.flySpeed * 0.1f);

                if (Input.mouseScrollDelta.y != 0)
                {
                    GlobalUtils.flySpeed += (int)Input.mouseScrollDelta.y;
                    //UI.resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
                }

                GlobalUtils.walkSpeed = 0;
                GlobalUtils.UpdatePlayerSpeed();
            }
        }
    }
}
