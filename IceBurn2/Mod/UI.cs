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

namespace IceBurn.Mod
{
    class UI : VRmod
    {
        public override string Name => "User Interface";
        public override string Description => "creates buttons in User Interface";

        // Менюшки
        public static QMNestedButton mainMenuP1;
        public static QMNestedButton mainMenuP2;
        public static QMNestedButton flyMenu;
        public static QMNestedButton FOVChangerMenu;
        public static QMNestedButton teleportMenu;
        public static QMNestedButton followMenu;
        public static QMNestedButton speedHackMenu;

        // Кнопки Основного меню
        public static QMToggleButton toggleESP;
        public static QMToggleButton toggleEarRape;
        public static QMSingleButton deleteAllPortals;
        public static QMToggleButton toggleFakeNameSpaces;
        public static QMToggleButton toggleAudioBitrate;
        public static QMSingleButton reconnectInstance;

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

        // Кнопки SpeedHack
        public static QMSingleButton resetWalkSpeed;
        public static QMHalfButton WalkSpeedUp;
        public static QMHalfButton WalkSpeedDown;
        public static QMHalfButton WalkSpeedUpX;
        public static QMHalfButton WalkSpeedDownX;
        public static QMSingleButton ohShiitWalk;

        // Другие кнопки
        public static QMSingleButton forceClone;
        public static QMSingleButton crashCheck;
        public static QMSingleButton downloadVRCA;
        public static QMSingleButton quitApp;

        public override void OnStart()
        {
            // Инициализация меню
            mainMenuP1 = new QMNestedButton("ShortcutMenu", 5, 2, "Utils", "Ice Burn Utils");
            mainMenuP2 = new QMNestedButton(mainMenuP1, 5, 1, "Next\nPage", "Page 2");
            flyMenu = new QMNestedButton(mainMenuP1, 1, 0, "Fly\nMenu", "Fly Menu");
            FOVChangerMenu = new QMNestedButton(mainMenuP1, 1, 1, "FOV\nMenu", "Field Of View Menu");
            speedHackMenu = new QMNestedButton(mainMenuP1, 1, 2, "Player\nSpeed", "Speed Hack Menu");

            // Это просто нужно
            mainMenuP2.getBackButton().setButtonText("Previous\nPage");

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
                Physics.gravity = Vector3.zero;
                GlobalUtils.ToggleColliders(false);

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
                Physics.gravity = GlobalUtils.Gravity;
                GlobalUtils.ToggleColliders(true);

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

                IceLogger.Log("ESP has been Enabled");
            }), "ESP OFF", new Action(() =>
            {
                GlobalUtils.ESP = false;

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

            forceClone = new QMSingleButton("UserInteractMenu", 4, 2, "Force\nClone", new Action(() =>
            {
                try
                {
                    ApiAvatar avatar = Wrapper.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0;

                    if (avatar.releaseStatus != "private")
                        new PageAvatar { avatar = new SimpleAvatarPedestal { field_Internal_ApiAvatar_0 = new ApiAvatar { id = avatar.id } } }.ChangeToSelectedAvatar();
                    else
                        IceLogger.Log("Avatar release status is PRIVATE!");
                }
                catch (Exception)
                {
                    IceLogger.Error("User not selected!");
                    throw;
                }
            }), "Force Clone User Avatar");

            crashCheck = new QMSingleButton("UserInteractMenu", 3, 3, "Crash\nCheck", new Action(() =>
            {
                anticrash.particle_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.polygon_check(Wrapper.GetQuickMenu().GetSelectedPlayer(), Wrapper.get_poly(Wrapper.GetQuickMenu().GetSelectedPlayer()));
                anticrash.shader_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.mesh_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.mats_check(Wrapper.GetQuickMenu().GetSelectedPlayer());
                anticrash.work_hk(Wrapper.GetQuickMenu().GetSelectedPlayer(), Wrapper.get_poly(Wrapper.GetQuickMenu().GetSelectedPlayer()));
                IceLogger.Log("Player Checked");
            }), "Force Clone User Avatar");

            downloadVRCA = new QMSingleButton("UserInteractMenu", 4, 3, "Download\nVRCA", new Action(() =>
            {
                Process.Start(Wrapper.GetQuickMenu().GetSelectedPlayer().field_Private_VRCAvatarManager_0.field_Private_AvatarPerformanceStats_0.field_Public_String_0);
            }), "Force Clone User Avatar");

            /*resetSelectedPlayerVolume = new QMSingleButton("UserInteractMenu", 1, 3, "Reset\nVolume\n[]", new Action(() =>
            {
                Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_USpeaker_0.SpeakerVolume = 1f;
                resetSelectedPlayerVolume.setButtonText("Reset\nVolume\n[" + Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_USpeaker_0.SpeakerVolume + "]");
            }), "Reset " + Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_String_0 + " Volume [100]");

            selectedPlayerVolumeUp = new QMHalfButton("UserInteractMenu", 2, 2.5f, "▲", new Action(() =>
            {
                Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_USpeaker_0.SpeakerVolume += 0.1f;
                resetSelectedPlayerVolume.setButtonText("Reset\nVolume\n[" + Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_USpeaker_0.SpeakerVolume + "]");
            }), "Selected Player Volume Up");

            selectedPlayerVolumeDown = new QMHalfButton("UserInteractMenu", 2, 3.5f, "▼", new Action(() =>
            {
                Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_USpeaker_0.SpeakerVolume -= 0.1f;
                resetSelectedPlayerVolume.setButtonText("Reset\nVolume\n[" + Wrapper.GetSelectedPlayer(Wrapper.GetQuickMenu()).field_Private_USpeaker_0.SpeakerVolume + "]");
            }), "Selected Player Volume Up");*/

            teleportMenu = new QMNestedButton(mainMenuP1, 2, 0, "Teleport", new Action(() =>
            {
                var players = PlayerWrapper.GetAllPlayers(Wrapper.GetPlayerManager()).ToArray();

                int localX = 1;
                float localY = -0.5f;

                for (int i = 0; i < players.Length; i++)
                {
                    IceLogger.Log("PlayerList:");
                    IceLogger.Log(players[i].ToString());
                    new QMHalfButton(teleportMenu, localX, localY, players[i].ToString(), new Action(() =>
                    {
                        PlayerWrapper.GetCurrentPlayer().transform.position = players[i].transform.position;
                        IceLogger.Log("SelectedPlayer: " + players[i].ToString());
                    }), "Teleport To " + players[i].ToString());

                    localX++;

                    if (localX > 4
                    )
                    {
                        localX = 1;
                        localY += 1f;
                    }
                }
            }), "Teleport To Player");

            followMenu = new QMNestedButton(mainMenuP1, 2, 1, "Follow\nPlayer", new Action(() =>
            {
                new QMSingleButton(followMenu, 1, 0, "This is Follow menu", new Action(() => { IceLogger.Log("Test BUTTON Clicked"); }), "TEST INFO BUTTON");
            }), "Select Target And Follow IT! :D");

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
                Application.Quit();
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

            deleteAllPortals = new QMSingleButton(mainMenuP1, 3, 1, "Delete\nPortals", new Action(() =>
            {
                (from portal in Resources.FindObjectsOfTypeAll<PortalInternal>()
                 where portal.gameObject.activeInHierarchy && !portal.gameObject.GetComponentInParent<VRC_PortalMarker>()
                 select portal).ToList<PortalInternal>().ForEach(delegate (PortalInternal p)
                 {
                     Player component = Networking.GetOwner(p.gameObject).gameObject.GetComponent<Player>();
                     if (component != null && PlayerWrapper.is_friend(component))
                     {
                         return;
                     }
                     UnityEngine.Object.Destroy(p.transform.root.gameObject);
                 });
            }), "Delete All Portals");

            toggleFakeNameSpaces = new QMToggleButton(mainMenuP1, 4, 1, "Fake NameSpace", new Action(() =>
            {
                var allPlayers = PlayerWrapper.GetAllPlayers(Wrapper.GetPlayerManager());
                for (int i = 0; i < allPlayers.Count; i++)
                    allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.red);
            }), "RealNameSpace", new Action(() =>
            {
                var allPlayers = PlayerWrapper.GetAllPlayers(Wrapper.GetPlayerManager());
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    IceLogger.Log("field_Private_String_0: " + allPlayers[i].field_Internal_VRCPlayer_0);
                    IceLogger.Log("field_Private_APIUser_0: " + allPlayers[i].field_Private_APIUser_0);
                    IceLogger.Log("field_Private_VRCAvatarManager_0: " + allPlayers[i].field_Private_VRCAvatarManager_0);
                    IceLogger.Log("field_Private_VRCPlayerApi_0: " + allPlayers[i].field_Private_VRCPlayerApi_0);
                    IceLogger.Log("prop_VRCPlayerApi_0: " + allPlayers[i].prop_VRCPlayerApi_0);
                    IceLogger.Log("prop_String_0: " + allPlayers[i].prop_String_0);
                    IceLogger.Log("prop_String_1: " + allPlayers[i].prop_String_1);
                    IceLogger.Log("tag: " + allPlayers[i].tag);
                    //allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.red);
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

            // Initial State
            toggleFly.setToggleState(false);
            flySpeedUp.setActive(false);
            flySpeedDown.setActive(false);
            resetflySpeed.setActive(false);
            flySpeedUpX.setActive(false);
            flySpeedDownX.setActive(false);
            ohShiitFly.setActive(false);

            if (VRCTrackingManager.Method_Public_Static_Boolean_11())
                FOVChangerMenu.getMainButton().setActive(false);
            else
                FOVChangerMenu.getMainButton().setActive(true);
        }
    }
}
