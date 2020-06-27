using System;
using IceBurn.API;
using IceBurn.Other;
using IceBurn.Utils;
using System.Linq;
using UnityEngine;
using VRC.SDKBase;
using VRC.Core;
using VRC;
using VRC.UI;
using System.Diagnostics;
using IceBurn.Mod.Other;
using UnityEngine.UI;
using System.Collections.Generic;
using Logger;

namespace IceBurn.Mod
{
    class UI : VRmod
    {
        // Менюшки
        public static QMNestedButton mainMenuP1;
        public static QMNestedButton mainMenuP2;
        public static QMNestedButton mainMenuP3;
        public static QMNestedButton flyMenu;
        public static QMNestedButton FOVChangerMenu;
        public static QMNestedButton teleportMenu;
        public static QMNestedButton pointTeleportMenu;
        public static QMNestedButton speedHackMenu;
        public static QMNestedButton userUtilsMenu;
        public static QMNestedButton brightnessMenu;
        public static QMNestedButton lightMenu;
        public static QMNestedButton dropPortalMenu;

        // Кнопки Основного меню
        public static QMToggleButton toggleESP;
        public static QMToggleButton toggleEarRape;
        public static QMSingleButton hideAllPortals;
        public static QMToggleButton toggleFakeNamePlate;
        public static QMToggleButton toggleAudioBitrate;
        public static QMSingleButton reconnectInstance;
        public static QMSingleButton test;

        // Кнопки Второго меню
        public static QMSingleButton addJump;
        public static QMSingleButton deleteAllPortals;
        public static QMToggleButton toggleShadows;
        public static QMToggleButton toggleOptimizeMirror;
        public static QMToggleButton toggleHand;
        public static QMToggleButton toggleGDB;

        // Кнопки Fly
        public static QMSingleButton resetflySpeed;
        public static QMHalfButton flySpeedUp;
        public static QMHalfButton flySpeedDown;
        public static QMHalfButton flySpeedUpX;
        public static QMHalfButton flySpeedDownX;
        public static QMToggleButton toggleFly;
        public static QMSingleButton ohShiitFly;

        // Кнопки FOV
        public static QMSingleButton resetFOV;
        public static QMHalfButton FOVUp;
        public static QMHalfButton FOVDown;
        public static QMHalfButton FOVUpX;
        public static QMHalfButton FOVDownX;

        // Кнопки PointTeleportmenu
        public static QMHalfButton createNewPointToTeleport;
        public static QMHalfButton removeAllPointsToTeleport;

        // Кнопки brightnessMenu
        public static QMSingleButton resetBrightness;
        public static QMHalfButton brightnessUp;
        public static QMHalfButton brightnessDown;

        // Кнопки SpeedHack
        public static QMSingleButton resetWalkSpeed;
        public static QMHalfButton WalkSpeedUp;
        public static QMHalfButton WalkSpeedDown;
        public static QMHalfButton WalkSpeedUpX;
        public static QMHalfButton WalkSpeedDownX;
        public static QMSingleButton ohShiitWalk;

        // Кнопки User
        public static QMSingleButton forceClone;
        public static QMSingleButton crashCheck;
        public static QMSingleButton downloadVRCA;

        // Кнопки OwnLight
        public static QMToggleButton toggleOwnLight;
        public static QMToggleButton toggleOwnLightShadows;
        public static QMHalfButton ownLightIntUp;
        public static QMHalfButton ownLightIntDown;
        public static QMSingleButton ownLightIntReset;
        public static QMSingleButton ownLightAdd;

        // Другие кнопки
        public static QMSingleButton quitApp;

        // XXX
        private static List<QMHalfButton> tPlayerList = new List<QMHalfButton>();
        private static List<Player> tmpPlayerList = new List<Player>();

        // TeleportPoints
        private static List<GameObject> pointTeleportList = new List<GameObject>();
        private static List<QMHalfButton> tPointList = new List<QMHalfButton>();

        // Hands
        //static RootMotion.FinalIK.VRIK controller;
        //public static Hand hand = Hand.None;

        //PlayerLight
        private static Light PlayerLight = new Light();

        public override void OnStart()
        {
            // Инициализация меню
            mainMenuP1 = new QMNestedButton("ShortcutMenu", 5, 2, "Utils", "Ice Burn Utils");
            mainMenuP2 = new QMNestedButton(mainMenuP1, 5, 1, "Next\nPage", "Page 2");
            mainMenuP3 = new QMNestedButton(mainMenuP2, 5, 1, "Next\nPage", "Page 3");
            flyMenu = new QMNestedButton(mainMenuP1, 1, 0, "Fly\nMenu", "Fly Menu");
            FOVChangerMenu = new QMNestedButton(mainMenuP1, 1, 1, "FOV\nMenu", "Field Of View Menu");
            speedHackMenu = new QMNestedButton(mainMenuP1, 1, 2, "Player\nSpeed", "Speed Hack Menu");
            brightnessMenu = new QMNestedButton(mainMenuP2, 1, 1, "Light\nIntensity", "Set Light Intensity");
            lightMenu = new QMNestedButton(mainMenuP2, 3, 1, "Light\nMenu", "User Light Menu");
            userUtilsMenu = new QMNestedButton("UserInteractMenu", 4, 2, "Utils", "User Utils");

            // Это просто нужно
            mainMenuP2.getBackButton().setButtonText("Previous\nPage");
            mainMenuP3.getBackButton().setButtonText("Previous\nPage");

            // Инициализация кнопок
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

            toggleESP = new QMToggleButton(mainMenuP1, 3, 0,
            "ESP ON", new Action(() =>
            {
                GlobalUtils.ESP = true;

                var allPlayers = PlayerWrapper.GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    Transform sRegion = allPlayers[i].transform.Find("SelectRegion");
                    if (sRegion != null)
                    {
                        sRegion.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);
                        Renderer testRenderer = allPlayers[i].field_Internal_VRCPlayer_0.namePlate.gameObject.GetComponent<Renderer>();
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(testRenderer, GlobalUtils.ESP);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(sRegion.GetComponent<Renderer>(), GlobalUtils.ESP);
                    }
                }

                IceLogger.Log("ESP has been Enabled");
            }), "ESP OFF", new Action(() =>
            {
                GlobalUtils.ESP = false;

                var allPlayers = PlayerWrapper.GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    Transform sRegion = allPlayers[i].transform.Find("SelectRegion");
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
                IceLogger.Log("ESP has been Disabled");
            }), "Toggle ESP");

            toggleEarRape = new QMToggleButton(mainMenuP1, 4, 0,
            "EarRape ON", new Action(() =>
            {
                USpeaker.field_Internal_Static_Single_1 = float.MaxValue;
                IceLogger.Log("EarRape Enabled");
            }), "EarRape OFF", new Action(() =>
            {
                USpeaker.field_Internal_Static_Single_1 = 1f;
                IceLogger.Log("EarRape Disabled");
            }), "Toggle EarRape");

            forceClone = new QMSingleButton(userUtilsMenu, 1, 0, "Force\nClone", new Action(() =>
            {
                try
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
                catch (Exception)
                {
                    IceLogger.Error("User not selected!");
                    throw;
                }
            }), "Force Clone User Avatar");

            crashCheck = new QMSingleButton(userUtilsMenu, 2, 0, "Crash\nCheck", new Action(() =>
            {
                anticrash.particle_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.polygon_check(Wrapper.GetQuickMenu().GetSelectedPlayer(), Wrapper.get_poly(Wrapper.GetQuickMenu().GetSelectedPlayer()));
                anticrash.shader_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.mesh_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.mats_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.work_hk(Wrapper.GetQuickMenu().GetSelectedPlayer(), Wrapper.get_poly(Wrapper.GetQuickMenu().GetSelectedPlayer()));
                IceLogger.Log("Player Checked");
            }), "Force Clone User Avatar");

            downloadVRCA = new QMSingleButton(userUtilsMenu, 3, 0, "Download\nVRCA", new Action(() =>
            {
                //Process.Start(Wrapper.GetQuickMenu().GetSelectedPlayer().field_Private_VRCAvatarManager_0.field_Private_AvatarPerformanceStats_0.field_Public_String_0);
                Process.Start(Wrapper.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0.assetUrl);
            }), "Force Clone User Avatar");

            teleportMenu = new QMNestedButton(mainMenuP1, 2, 0, "Teleport", new Action(() =>
            {
                PlayerWrapper.UpdateFriendList();

                // Remove old Buttons
                foreach (QMHalfButton item in tPlayerList)
                    item.DestroyMe();
                tPlayerList.Clear();

                // Get All Players
                var players = PlayerWrapper.GetAllPlayers();

                // REAdd Players to List
                tmpPlayerList.Clear();
                for (int i = 0; i < players.Count; i++)
                    tmpPlayerList.Add(players[i]);

                // Button Local Position
                int localX = 0;
                float localY = -0.5f;

                if (tmpPlayerList.Count <= 24)
                {
                    localX = 1;
                    foreach (Player player in tmpPlayerList)
                    {
                        QMHalfButton tmpButton = new QMHalfButton(teleportMenu, localX, localY, player.ToString(), new Action(() =>
                        {
                            try
                            {
                                IceLogger.Log("Trying Teleport TO: [" + player.ToString() + "]");
                                PlayerWrapper.GetCurrentPlayer().transform.position = player.transform.position;
                            }
                            catch (Exception ex)
                            {
                                IceLogger.Error(ex.ToString());
                            }
                        }), "Teleport To " + player.ToString());

                        if (PlayerWrapper.isFriend(player.field_Internal_VRCPlayer_0.prop_Player_0))
                            tmpButton.setTextColor(Color.green);
                        else
                            tmpButton.setTextColor(Color.white);

                        if (PlayerWrapper.GetTrustLevel(player) == "Veteran user")
                            tmpButton.setBackgroundColor(Color.red);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Trusted user")
                            tmpButton.setBackgroundColor(Color.magenta);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Known user")
                            tmpButton.setBackgroundColor(Color.Lerp(Color.yellow, Color.red, 0.5f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "User")
                            tmpButton.setBackgroundColor(Color.green);
                        else if (PlayerWrapper.GetTrustLevel(player) == "New user")
                            tmpButton.setBackgroundColor(new Color(0.19f, 0.45f, 0.62f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "Visitor")
                            tmpButton.setBackgroundColor(Color.gray);

                        if (player.field_Private_APIUser_0.id == "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223" || player.field_Internal_VRCPlayer_0.prop_String_1 == PlayerWrapper.GetCurrentPlayer().prop_String_1)
                        {
                            tmpButton.setBackgroundColor(Color.black);
                            tmpButton.setTextColor(Color.red);
                        }

                        localX++;
                        if (localX > 4)
                        {
                            localX = 1;
                            localY += 1f;
                        }
                        tPlayerList.Add(tmpButton);
                    }
                }
                else
                {
                    foreach (Player player in tmpPlayerList)
                    {
                        QMHalfButton tmpButton = new QMHalfButton(teleportMenu, localX, localY, player.ToString(), new Action(() =>
                        {
                            try
                            {
                                IceLogger.Log("Trying Teleport TO: [" + player.ToString() + "]");
                                PlayerWrapper.GetCurrentPlayer().transform.position = player.transform.position;
                            }
                            catch (Exception ex)
                            {
                                IceLogger.Error(ex.ToString());
                            }
                        }), "Teleport To " + player.ToString());

                        if (PlayerWrapper.isFriend(player.field_Internal_VRCPlayer_0.prop_Player_0))
                            tmpButton.setTextColor(Color.green);
                        else
                            tmpButton.setTextColor(Color.white);

                        if (PlayerWrapper.GetTrustLevel(player) == "Veteran user")
                            tmpButton.setBackgroundColor(Color.red);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Trusted user")
                            tmpButton.setBackgroundColor(Color.magenta);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Known user")
                            tmpButton.setBackgroundColor(Color.Lerp(Color.yellow, Color.red, 0.5f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "User")
                            tmpButton.setBackgroundColor(Color.green);
                        else if (PlayerWrapper.GetTrustLevel(player) == "New user")
                            tmpButton.setBackgroundColor(new Color(0.19f, 0.45f, 0.62f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "Visitor")
                            tmpButton.setBackgroundColor(Color.gray);

                        if (player.field_Private_APIUser_0.id == "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223" || player.field_Internal_VRCPlayer_0.prop_String_1 == PlayerWrapper.GetCurrentPlayer().prop_String_1)
                        {
                            tmpButton.setBackgroundColor(Color.black);
                            tmpButton.setTextColor(Color.red);
                        }

                        localX++;
                        if (localX > 5 && localY < 4f)
                        {
                            localX = 0;
                            localY += 1f;
                        }
                        else if (localX > 5 && localY > 2f)
                        {
                            localX = 1;
                            localY += 1f;
                        }
                        tPlayerList.Add(tmpButton);
                    }
                }
            }), "Teleport To Player");

            dropPortalMenu = new QMNestedButton(mainMenuP2, 4, 1, "Drop\nPortal", new Action(() =>
            {
                PlayerWrapper.UpdateFriendList();

                // Remove old Buttons
                foreach (QMHalfButton item in tPlayerList)
                    item.DestroyMe();
                tPlayerList.Clear();

                // Get All Players
                var players = PlayerWrapper.GetAllPlayers();

                // REAdd Players to List
                tmpPlayerList.Clear();
                for (int i = 0; i < players.Count; i++)
                    tmpPlayerList.Add(players[i]);

                // Button Local Position
                int localX = 0;
                float localY = -0.5f;

                if (tmpPlayerList.Count <= 24)
                {
                    localX = 1;
                    foreach (Player player in tmpPlayerList)
                    {
                        QMHalfButton tmpButton = new QMHalfButton(dropPortalMenu, localX, localY, player.ToString(), new Action(() =>
                        {
                            if (player.field_Private_APIUser_0.id != "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223" || player.field_Internal_VRCPlayer_0.prop_String_1 != PlayerWrapper.GetCurrentPlayer().prop_String_1)
                                try
                                {
                                    IceLogger.Log("Trying Drop TO: [" + player.ToString() + "]");
                                    GameObject portal = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", player.transform.position, player.transform.rotation);
                                    Networking.RPC(RPC.Destination.AllBufferOne, portal, "ConfigurePortal", new Il2CppSystem.Object[]
                                    {
                                        (Il2CppSystem.String)"wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc",
                                        //(Il2CppSystem.String)Clipboard.GetText(),
                                        (Il2CppSystem.String)"99999",
                                        new Il2CppSystem.Int32
                                        {
                                            m_value = 0
                                        }.BoxIl2CppObject()
                                    });
                                }
                                catch (Exception ex)
                                {
                                    IceLogger.Error(ex.ToString());
                                }
                        }), "Drop Portal To " + player.ToString());

                        if (PlayerWrapper.isFriend(player.field_Internal_VRCPlayer_0.prop_Player_0))
                            tmpButton.setTextColor(Color.green);
                        else
                            tmpButton.setTextColor(Color.white);

                        if (PlayerWrapper.GetTrustLevel(player) == "Veteran user")
                            tmpButton.setBackgroundColor(Color.red);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Trusted user")
                            tmpButton.setBackgroundColor(Color.magenta);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Known user")
                            tmpButton.setBackgroundColor(Color.Lerp(Color.yellow, Color.red, 0.5f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "User")
                            tmpButton.setBackgroundColor(Color.green);
                        else if (PlayerWrapper.GetTrustLevel(player) == "New user")
                            tmpButton.setBackgroundColor(new Color(0.19f, 0.45f, 0.62f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "Visitor")
                            tmpButton.setBackgroundColor(Color.gray);

                        if (player.field_Private_APIUser_0.id == "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223" || player.field_Internal_VRCPlayer_0.prop_String_1 == PlayerWrapper.GetCurrentPlayer().prop_String_1)
                        {
                            tmpButton.setBackgroundColor(Color.black);
                            tmpButton.setTextColor(Color.red);
                        }

                        localX++;
                        if (localX > 4)
                        {
                            localX = 1;
                            localY += 1f;
                        }
                        tPlayerList.Add(tmpButton);
                    }
                }
                else
                {
                    foreach (Player player in tmpPlayerList)
                    {
                        QMHalfButton tmpButton = new QMHalfButton(dropPortalMenu, localX, localY, player.ToString(), new Action(() =>
                        {
                            if (player.field_Private_APIUser_0.id != "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223" || player.field_Internal_VRCPlayer_0.prop_String_1 != PlayerWrapper.GetCurrentPlayer().prop_String_1)
                                try
                                {
                                    IceLogger.Log("Trying Drop TO: [" + player.ToString() + "]");
                                    GameObject portal = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", player.transform.position, player.transform.rotation);
                                    Networking.RPC(RPC.Destination.AllBufferOne, portal, "ConfigurePortal", new Il2CppSystem.Object[]
                                    {
                                        (Il2CppSystem.String)"wrld_5b89c79e-c340-4510-be1b-476e9fcdedcc",
                                        (Il2CppSystem.String)"99999",
                                        new Il2CppSystem.Int32
                                        {
                                            m_value = 0
                                        }.BoxIl2CppObject()
                                    });
                                }
                                catch (Exception ex)
                                {
                                    IceLogger.Error(ex.ToString());
                                }
                        }), "Teleport To " + player.ToString());

                        if (PlayerWrapper.isFriend(player.field_Internal_VRCPlayer_0.prop_Player_0))
                            tmpButton.setTextColor(Color.green);
                        else
                            tmpButton.setTextColor(Color.white);

                        if (PlayerWrapper.GetTrustLevel(player) == "Veteran user")
                            tmpButton.setBackgroundColor(Color.red);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Trusted user")
                            tmpButton.setBackgroundColor(Color.magenta);
                        else if (PlayerWrapper.GetTrustLevel(player) == "Known user")
                            tmpButton.setBackgroundColor(Color.Lerp(Color.yellow, Color.red, 0.5f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "User")
                            tmpButton.setBackgroundColor(Color.green);
                        else if (PlayerWrapper.GetTrustLevel(player) == "New user")
                            tmpButton.setBackgroundColor(new Color(0.19f, 0.45f, 0.62f));
                        else if (PlayerWrapper.GetTrustLevel(player) == "Visitor")
                            tmpButton.setBackgroundColor(Color.gray);

                        if (player.field_Private_APIUser_0.id == "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223" || player.field_Internal_VRCPlayer_0.prop_String_1 == PlayerWrapper.GetCurrentPlayer().prop_String_1)
                        {
                            tmpButton.setBackgroundColor(Color.black);
                            tmpButton.setTextColor(Color.red);
                        }

                        localX++;
                        if (localX > 5 && localY < 4f)
                        {
                            localX = 0;
                            localY += 1f;
                        }
                        else if (localX > 5 && localY > 2f)
                        {
                            localX = 1;
                            localY += 1f;
                        }
                        tPlayerList.Add(tmpButton);
                    }
                }
            }), "Teleport To Player");

            void updatepointlist()
            {
                // Remove old Buttons
                foreach (QMHalfButton item in tPointList)
                    item.DestroyMe();
                tPointList.Clear();

                removeAllPointsToTeleport.setTextColor(Color.red);
                if (pointTeleportList.Count < 24)
                    createNewPointToTeleport.setActive(true);
                else
                    createNewPointToTeleport.setActive(false);

                int localX = 1;
                float localY = -0.5f;

                foreach (GameObject point in pointTeleportList)
                {
                    QMHalfButton tmpButton = new QMHalfButton(pointTeleportMenu, localX, localY, point.name, new Action(() =>
                    {
                        try
                        {
                            IceLogger.Log("Trying Teleport TO: [" + point.name + "]");
                            PlayerWrapper.GetCurrentPlayer().transform.position = point.transform.position;
                        }
                        catch (Exception ex)
                        {
                            IceLogger.Error(ex.ToString());
                        }
                    }), "Teleport To " + point.name);

                    localX++;
                    if (localX > 4)
                    {
                        localX = 1;
                        localY += 1f;
                    }
                    tPointList.Add(tmpButton);
                }

                if (pointTeleportList.Count < 24)
                    createNewPointToTeleport.setActive(true);
                else
                    createNewPointToTeleport.setActive(false);
            }

            pointTeleportMenu = new QMNestedButton(mainMenuP1, 2, 1, "Point\nTeleport", new Action(() =>
            {
                updatepointlist();
            }), "Teleport TO Points");

            removeAllPointsToTeleport = new QMHalfButton(pointTeleportMenu, 5, 2.5f, "Remove All", new Action(() =>
            {
                foreach (QMHalfButton buttons in tPointList)
                    buttons.DestroyMe();
                tPointList.Clear();
                pointTeleportList.Clear();

                if (pointTeleportList.Count < 24)
                    createNewPointToTeleport.setActive(true);
                else
                    createNewPointToTeleport.setActive(false);
            }), "Remove All Points");

            createNewPointToTeleport = new QMHalfButton(pointTeleportMenu, 5, 1.5f, "Create New", new Action(() =>
            {
                Transform tmpPlayerTransform = PlayerWrapper.GetCurrentPlayer().gameObject.transform;
                GameObject tmpPointGO = Instantiate(new GameObject(), tmpPlayerTransform);
                Text tmpPointGOText = tmpPointGO.AddComponent<Text>();

                //tmpPointGOText.font = Resources.GetBuiltinResource(Il2CppSystem.Type.typeof(Font), "BankGothicLight.ttf") as Font;
                tmpPointGOText.text = PlayerWrapper.GetCurrentPlayer().transform.position.ToString();
                tmpPointGO.name = tmpPointGOText.text;

                pointTeleportList.Add(tmpPointGO);
                updatepointlist();
            }), "Create New Point");

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

            quitApp = new QMSingleButton("UIElementsMenu", 5, 2, "Quit\nGame", new Action(() =>
            {
                UnityEngine.Application.Quit();
            }), "Quit Game", null, Color.red);

            ohShiitFly = new QMSingleButton(flyMenu, 1, 2, "SHEEET", new Action(() =>
            {
                GlobalUtils.flySpeed += 1000;
                resetflySpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.flySpeed + "]");
            }), "OH SHEEEEEEEEEEEEEEEEEEEET");

            resetWalkSpeed = new QMSingleButton(speedHackMenu, 1, 0, "Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]", new Action(() =>
            {
                GlobalUtils.walkSpeed = 2;
                GlobalUtils.UpdatePlayerSpeed();
                resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }), "Reset Speed To Default [2]");

            WalkSpeedUp = new QMHalfButton(speedHackMenu, 2, -0.5f, "▲", new Action(() =>
            {
                GlobalUtils.walkSpeed++;
                GlobalUtils.UpdatePlayerSpeed();
                resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }), "Speed UP");

            WalkSpeedDown = new QMHalfButton(speedHackMenu, 2, 0.5f, "▼", new Action(() =>
            {
                GlobalUtils.walkSpeed--;
                GlobalUtils.UpdatePlayerSpeed();
                resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }), "Speed DOWN");

            WalkSpeedUpX = new QMHalfButton(speedHackMenu, 3, -0.5f, "▲▲▲", new Action(() =>
            {
                GlobalUtils.walkSpeed += 3;
                GlobalUtils.UpdatePlayerSpeed();
                resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }), "Speed UP 3X");

            WalkSpeedDownX = new QMHalfButton(speedHackMenu, 3, 0.5f, "▼▼▼", new Action(() =>
            {
                GlobalUtils.walkSpeed -= 3;
                GlobalUtils.UpdatePlayerSpeed();
                resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }), "Speed DOWN 3X");

            ohShiitWalk = new QMSingleButton(speedHackMenu, 1, 2, "SHEEET", new Action(() =>
            {
                GlobalUtils.walkSpeed += 1000;
                GlobalUtils.UpdatePlayerSpeed();
                resetWalkSpeed.setButtonText("Reset\nSpeed\n[" + GlobalUtils.walkSpeed + "]");
            }), "OH SHEEEEEEEEEEEEEEEEEEEET");

            deleteAllPortals = new QMSingleButton(mainMenuP2, 2, 0, "Delete\nPortals", new Action(() =>
            {
                (from portal in Resources.FindObjectsOfTypeAll<PortalInternal>()
                 where portal.gameObject.activeInHierarchy && !portal.gameObject.GetComponentInParent<VRC_PortalMarker>()
                 select portal).ToList<PortalInternal>().ForEach(delegate (PortalInternal p)
                 {
                     Player component = Networking.GetOwner(p.gameObject).gameObject.GetComponent<Player>();
                     UnityEngine.Object.Destroy(p.transform.root.gameObject);
                 });
            }), "Delete All Portals");

            hideAllPortals = new QMSingleButton(mainMenuP1, 3, 1, "Hide Portals", new Action(() =>
            {
                (from portal in Resources.FindObjectsOfTypeAll<PortalInternal>()
                 where portal.gameObject.activeInHierarchy && !portal.gameObject.GetComponentInParent<VRC_PortalMarker>()
                 select portal).ToList<PortalInternal>().ForEach(delegate (PortalInternal p)
                 {
                     Player component = Networking.GetOwner(p.gameObject).gameObject.GetComponent<Player>();
                     p.transform.root.gameObject.SetActive(false);
                 });
            }), "Hide All Portals");

            toggleFakeNamePlate = new QMToggleButton(mainMenuP1, 4, 1, "Fake Nameplate", new Action(() =>
            {
                var allPlayers = PlayerWrapper.GetAllPlayers();
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.red);
                    allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.red;
                    allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.red;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.red;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.clear;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlateTalkSprite = allPlayers[i].field_Internal_VRCPlayer_0.namePlateSilentSprite;
                }
            }), "Real Nameplate", new Action(() =>
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
            }), "Toggle Fake NameSpace");

            toggleAudioBitrate = new QMToggleButton(mainMenuP1, 2, 2, "64kbps", new Action(() =>
            {
                PlayerWrapper.GetCurrentPlayer().field_Private_USpeaker_0.CurrentBitrate = EnumPublicSealedvaBi15BiBiBiBiBiBiBiUnique.BitRate_64k;
            }), "24kbps", new Action(() =>
            {
                PlayerWrapper.GetCurrentPlayer().field_Private_USpeaker_0.CurrentBitrate = EnumPublicSealedvaBi15BiBiBiBiBiBiBiUnique.BitRate_24K;
            }), "Toggle Audio Bitrate");

            reconnectInstance = new QMSingleButton(mainMenuP1, 3, 2, "Reconnect", new Action(() =>
            {
                Networking.GoToRoom(Wrapper.GetInstance().instanceWorld.id + ":" + Wrapper.GetInstance().instanceWorld.currentInstanceIdWithTags);
            }), "Reconnect to instance");

            test = new QMSingleButton(mainMenuP1, 4, 2, "Test", new Action(() =>
            {
                var allPlayers = PlayerWrapper.GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_0);
                    IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_1);
                    var usertags = allPlayers[i].GetAPIUser().tags;
                    foreach (var tags in usertags)
                    {
                        IceLogger.Log(allPlayers[i].ToString() + " " + tags);
                    }
                    //Status: IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.field_Internal_String_1.ToString());
                }

                IceLogger.Log(PlayerWrapper.GetCurrentPlayer().namePlate.mainText.font.name);

                /*var allPlayers = Wrapper.GetPlayerManager().GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    Image someImage = allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.gameObject.AddComponent<Image>();
                    someImage.rectTransform.anchoredPosition += new Vector2(-1f, 0f);

                    float distance = Vector3.Distance(PlayerWrapper.GetCurrentPlayer().transform.position, allPlayers[i].transform.position);
                    IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_0 + ": " + distance);

                    IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_0 + ": " + allPlayers[i].field_Internal_VRCPlayer_0.prop_String_1);
                    var usertags = allPlayers[i].GetAPIUser().tags;
                    foreach (var tags in usertags)
                    {
                        IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_0 + ": " + tags);
                    }
                }*/
            }), "Test");

            toggleShadows = new QMToggleButton(mainMenuP2, 3, 0, "Shadows ON", new Action(() =>
            {
                foreach (Light light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    light.shadows = LightShadows.Soft;
                    light.shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
                }
            }), "Shadows OFF", new Action(() =>
            {
                foreach (Light light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    light.shadows = LightShadows.None;
                }
            }), "Toggle Shadows on map");

            addJump = new QMSingleButton(mainMenuP2, 1, 0, "Add\nJump", new Action(() =>
            {
                PlayerWrapper.GetCurrentPlayer().gameObject.AddComponent<PlayerModComponentJump>();
            }), "Add JumpComponent to you");

            toggleOptimizeMirror = new QMToggleButton(mainMenuP2, 4, 0, "Optimized Mirror", new Action(() =>
            {
                MirrorReflection[] array = UnityEngine.Object.FindObjectsOfType<MirrorReflection>();
                LayerMask mask = new LayerMask();
                mask.value = 263680;

                for (int i = 0; i < array.Length; i++)
                {
                    array[i].m_ReflectLayers = mask;//.value = 263680;
                }
                VRCSDK2.VRC_MirrorReflection[] array2 = UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_MirrorReflection>();
                for (int i = 0; i < array2.Length; i++)
                {
                    array2[i].m_ReflectLayers = mask;//.value = 263680;
                }

                VRC_MirrorReflection[] array4 = UnityEngine.Object.FindObjectsOfType<VRC_MirrorReflection>();
                for (int i = 0; i < array4.Length; i++)
                {
                    array4[i].m_ReflectLayers = mask;//.value = -1025;
                }
            }), "Normal Mirror", new Action(() =>
            {
                MirrorReflection[] array = UnityEngine.Object.FindObjectsOfType<MirrorReflection>();
                LayerMask mask = new LayerMask();
                mask.value = -1025;

                for (int i = 0; i < array.Length; i++)
                {
                    array[i].m_ReflectLayers = mask;//.value = 263680;
                }
                VRCSDK2.VRC_MirrorReflection[] array2 = UnityEngine.Object.FindObjectsOfType<VRCSDK2.VRC_MirrorReflection>();
                for (int i = 0; i < array2.Length; i++)
                {
                    array2[i].m_ReflectLayers = mask;//.value = 263680;
                }

                VRC_MirrorReflection[] array4 = UnityEngine.Object.FindObjectsOfType<VRC_MirrorReflection>();
                for (int i = 0; i < array4.Length; i++)
                {
                    array4[i].m_ReflectLayers = mask;//.value = -1025;
                }
            }), "Toggle Shadows on map");

            brightnessUp = new QMHalfButton(brightnessMenu, 2, -0.5f, "▲", new Action(() =>
            {
                GlobalUtils.brightness += (1f / 10f);
                foreach (Light light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    light.intensity = GlobalUtils.brightness;
                }
                resetBrightness.setButtonText("Reset\nBrightness\n[" + GlobalUtils.brightness + "]");
            }), "Brightness Up");

            brightnessDown = new QMHalfButton(brightnessMenu, 2, 0.5f, "▼", new Action(() =>
            {
                GlobalUtils.brightness -= (1f / 10f);
                foreach (Light light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    light.intensity = GlobalUtils.brightness;
                }
                resetBrightness.setButtonText("Reset\nBrightness\n[" + GlobalUtils.brightness + "]");
            }), "Brightness Down");

            resetBrightness = new QMSingleButton(brightnessMenu, 1, 0, "Reset\nBrightness\n[" + GlobalUtils.brightness + "]", new Action(() =>
            {
                GlobalUtils.brightness = 1f;
                foreach (Light light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    light.intensity = GlobalUtils.brightness;
                }
                resetBrightness.setButtonText("Reset\nBrightness\n[" + GlobalUtils.brightness + "]");
            }), "Reset To Default Brightness");

            toggleHand = new QMToggleButton(mainMenuP2, 2, 1, "Hand ON", new Action(() =>
            {
                /*controller = PlayerWrapper.GetCurrentPlayer().prop_Player_0.prop_VRCAvatarManager_0.prop_GameObject_0.GetComponent<RootMotion.FinalIK.VRIK>();
                if (Input.GetMouseButton(1))
                {
                    if (controller != null)
                    {
                        switch (hand)
                        {
                            case Hand.Left:
                                controller.solver.leftArm.positionWeight = 1;
                                controller.solver.leftArm.rotationWeight = 1;
                                break;
                            case Hand.Right:
                                controller.solver.rightArm.positionWeight = 1;
                                controller.solver.rightArm.rotationWeight = 1;
                                break;
                            case Hand.Both:
                                controller.solver.leftArm.positionWeight = 1;
                                controller.solver.leftArm.rotationWeight = 1;
                                controller.solver.rightArm.positionWeight = 1;
                                controller.solver.rightArm.rotationWeight = 1;
                                break;
                            default:
                                break;
                        }
                    }
                }*/
            }), "Hand OFF", new Action(() =>
            {

            }), "Toggle Sphere For Desktop Hand");

            toggleOwnLight = new QMToggleButton(lightMenu, 1, 0, "Light ON", new Action(() =>
            {
                PlayerLight.enabled = true;
            }), "Light OFF", new Action(() =>
            {
                PlayerLight.enabled = false;
            }), "Toggle Own Light");

            toggleOwnLightShadows = new QMToggleButton(lightMenu, 1, 1, "Shadows ON", new Action(() =>
            {
                PlayerLight.shadows = LightShadows.Soft;
                PlayerLight.shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh;
            }), "Shadows OFF", new Action(() =>
            {
                PlayerLight.shadows = LightShadows.None;
            }), "Toggle Own Shadow");

            ownLightIntUp = new QMHalfButton(lightMenu, 2, -0.5f, "▲", new Action(() =>
            {
                GlobalUtils.ownBrightness += 1f / 10f;
                ownLightIntReset.setButtonText("Reset\nInt\n[" + GlobalUtils.ownBrightness + "]");
                PlayerLight.intensity = GlobalUtils.ownBrightness;
            }), "Light Int UP");

            ownLightIntDown = new QMHalfButton(lightMenu, 2, 0.5f, "▼", new Action(() =>
            {
                if (GlobalUtils.ownBrightness <= 0)
                    GlobalUtils.ownBrightness = 0.1f;
                GlobalUtils.ownBrightness -= 1f / 10f;
                ownLightIntReset.setButtonText("Reset\nInt\n[" + GlobalUtils.ownBrightness + "]");
                PlayerLight.intensity = GlobalUtils.ownBrightness;
            }), "Light Int DOWN");

            ownLightIntReset = new QMSingleButton(lightMenu, 3, 0, "Reset\nInt\n[" + GlobalUtils.ownBrightness + "]", new Action(() =>
            {
                GlobalUtils.ownBrightness = 1f;
                PlayerLight.intensity = GlobalUtils.ownBrightness;
                ownLightIntReset.setButtonText("Reset\nInt\n[" + GlobalUtils.ownBrightness + "]");
            }), "Reset Own Light Int");

            ownLightAdd = new QMSingleButton(lightMenu, 4, 0, "Init\nLight", new Action(() =>
            {
                VRCPlayer player = PlayerWrapper.GetCurrentPlayer();
                GameObject def = Instantiate(new GameObject() , player.transform);
                def.transform.position = player.transform.position + (player.transform.forward * 0.5f) + player.transform.up;
                PlayerLight = def.AddComponent<Light>();
                PlayerLight.type = LightType.Point;
                PlayerLight.intensity = 1.0f;
                PlayerLight.enabled = false;
                IceLogger.Log("Light XPos: " + def.transform.position.x);
                IceLogger.Log("Light YPos: " + def.transform.position.y);
                IceLogger.Log("Light ZPos: " + def.transform.position.z);
            }), "USE ONE TIME!");

            toggleGDB = new QMToggleButton(mainMenuP2, 1, 2, "GDB ON", new Action(() =>
            {
                Bones.GDB.ToggleGDBState();
            }), "GDB OFF", new Action(() =>
            {
                Bones.GDB.ToggleGDBState();
            }), "Toggle GlobalDynamicBones");


            // Initial State
            toggleFly.setToggleState(false);
            flySpeedUp.setActive(false);
            flySpeedDown.setActive(false);
            resetflySpeed.setActive(false);
            flySpeedUpX.setActive(false);
            flySpeedDownX.setActive(false);
            ohShiitFly.setActive(false);

            if (VRCTrackingManager.Method_Public_Static_Boolean_11())
            {
                FOVChangerMenu.getMainButton().setBackgroundColor(Color.gray);
                FOVChangerMenu.getMainButton().setTextColor(Color.red);
                FOVChangerMenu.getMainButton().setAction(null);

                /*toggleHand.setBackgroundColor(Color.gray);
                toggleHand.setTextColor(Color.red);
                toggleHand.setAction(null, null);*/
            }
            else
            {
                FOVChangerMenu.getMainButton().setActive(true);
                FOVChanger.isPC = false;
            }
        }
    }
}
