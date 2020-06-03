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
using System.Windows.Forms;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace IceBurn.Mod
{
    class UI : VRmod
    {
        public override string Name => "QM User Interface";
        public override string Description => "creates buttons in QM User Interface";

        // Менюшки
        public static QMNestedButton mainMenuP1;
        public static QMNestedButton mainMenuP2;
        public static QMNestedButton mainMenuP3;
        public static QMNestedButton flyMenu;
        public static QMNestedButton FOVChangerMenu;
        public static QMNestedButton teleportMenu;
        public static QMNestedButton followMenu;
        public static QMNestedButton speedHackMenu;
        public static QMNestedButton userUtilsMenu;
        public static QMNestedButton brightnessMenu;

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
        public static QMSingleButton hideAllVisitors;
        public static QMToggleButton hideAllObjects;
        public static QMSingleButton selfCrashCheck;
        public static QMToggleButton invalid;

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

        // Другие кнопки
        public static QMSingleButton quitApp;

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

                var allPlayers = PlayerWrapper.GetAllPlayers(Wrapper.GetPlayerManager()).ToArray();
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

                var allPlayers = PlayerWrapper.GetAllPlayers(Wrapper.GetPlayerManager()).ToArray();
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
                        IceLogger.Log("Avatar release status is PRIVATE!");
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

            hideAllPortals = new QMSingleButton(mainMenuP1, 3, 1, "Hide\nPortals", new Action(() =>
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
                GlobalUtils.FakeNamePlate = true;
            }), "Real Nameplate", new Action(() =>
            {
                GlobalUtils.FakeNamePlate = false;
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
                /*var allPlayers = Wrapper.GetPlayerManager().GetAllPlayers().ToArray();
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
                }*/
                var allPlayers = Wrapper.GetPlayerManager().GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    float distance = Vector3.Distance(PlayerWrapper.GetCurrentPlayer().transform.position, allPlayers[i].transform.position);
                    IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_0 + ": " + distance);

                    IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_0 + ": " + allPlayers[i].field_Internal_VRCPlayer_0.prop_String_1);
                    var usertags = allPlayers[i].GetAPIUser().tags;
                    foreach (var tags in usertags)
                    {
                        IceLogger.Log(allPlayers[i].field_Internal_VRCPlayer_0.prop_String_0 + ": " + tags);
                    }
                }
            }), "Test");

            toggleShadows = new QMToggleButton(mainMenuP2, 3, 0, "Shadows ON", new Action(() =>
            {
                foreach (Light light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    light.shadows = LightShadows.Soft;
                    light.m_BakedIndex = 0;
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
                GlobalUtils.brightness += 0.1f;
                foreach (Light light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    light.intensity = GlobalUtils.brightness;
                }
                resetBrightness.setButtonText("Reset\nBrightness\n[" + GlobalUtils.brightness + "]");
            }), "Brightness Up");

            brightnessDown = new QMHalfButton(brightnessMenu, 2, 0.5f, "▼", new Action(() =>
            {
                GlobalUtils.brightness -= 0.1f;
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

            hideAllVisitors = new QMSingleButton(mainMenuP2, 2, 1, "Remove\nVisitors", new Action(() =>
            {
                var allPlayers = Wrapper.GetPlayerManager().GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Visitor")
                    {
                        PlayerWrapper.RemoveAvatar(allPlayers[i].field_Internal_VRCPlayer_0.field_Private_Player_0);
                        UnityEngine.GameObject.Destroy(allPlayers[i].field_Internal_VRCPlayer_0.namePlate.gameObject);
                    }
                }
            }), "Show Visitors");
            hideAllObjects = new QMToggleButton(mainMenuP2, 3, 1, "Hide Objects", new Action(() =>
            {
                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                    pickup.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }), "Show Objects", new Action(() =>
            {
                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                    pickup.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }), "Toggle VRC_Pickup Objects");

            selfCrashCheck = new QMSingleButton(mainMenuP2, 4, 1, "Self\nCrash\nCheck", new Action(() =>
            {
                anticrash.particle_check(PlayerWrapper.GetCurrentPlayer().prop_Player_0);
                anticrash.polygon_check(PlayerWrapper.GetCurrentPlayer().prop_Player_0, Wrapper.get_poly(PlayerWrapper.GetCurrentPlayer().prop_Player_0));
                anticrash.shader_check(PlayerWrapper.GetCurrentPlayer().prop_Player_0);
                anticrash.mesh_check(PlayerWrapper.GetCurrentPlayer().prop_Player_0);
                anticrash.mats_check(PlayerWrapper.GetCurrentPlayer().prop_Player_0);
                anticrash.work_hk(PlayerWrapper.GetCurrentPlayer().prop_Player_0, Wrapper.get_poly(PlayerWrapper.GetCurrentPlayer().prop_Player_0));
                IceLogger.Log("Player " + PlayerWrapper.GetCurrentPlayer().prop_Player_0.field_Private_String_0 + " Checked");
            }), "Self Crash Check");

            invalid = new QMToggleButton(mainMenuP2, 1, 2, "Invalid?", new Action(() =>
            {
                UnityEngine.Application.targetFrameRate = 10;
            }), "Maybe", new Action(() =>
            {
                UnityEngine.Application.targetFrameRate = 144;
            }), "Are you invalid blyat?");

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
