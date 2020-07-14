using IceBurn.API;
using IceBurn.Utils;
using IceBurn.Utils.Popup;
using IceModSystem;
using Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Transmtn.DTO.Notifications;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.UI;

namespace IceBurn.Mod
{
    public class UserUtils : VRmod
    {
        public override int LoadOrder => 11;

        public static QMNestedButton userUtilsMenu;

        public static QMSingleButton forceClone;
        public static QMSingleButton crashCheck;
        public static QMSingleButton downloadVRCA;
        public static QMSingleButton sendMessage;
        public static QMSingleButton GetAvatars;

        public override void OnStart()
        {
            userUtilsMenu = new QMNestedButton("UserInteractMenu", 4, 2, "Utils", "User Utils");

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
                        Popup.Alert("Clone ERROR!", "Avatar release status is PRIVATE!", "Back");
                        Console.Beep();
                    }
                    IceLogger.Log(avatar.id);
                }
                catch (Exception)
                {
                    IceLogger.Error("User not selected!");
                    Popup.Alert("Clone ERROR!", "User Not Selected\n[Select User To Clone]", "Back");
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
            }), "Check User Avatar to crash");

            downloadVRCA = new QMSingleButton(userUtilsMenu, 3, 0, "Download\nVRCA", new Action(() =>
            {
                Process.Start(Wrapper.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_ApiAvatar_0.assetUrl);
            }), "Download User Avatar in VRCA File");

            sendMessage = new QMSingleButton(userUtilsMenu, 4, 0, "Send\nMessage", new Action(() =>
            {
                try
                {
                    VRC.Player Splayer = Wrapper.GetQuickMenu().GetSelectedPlayer();

                    Popup.ShowUnityInputPopupWithCancel($"Send Message to: {Splayer.ToString()}", "", "Send", new Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>((string s, Il2CppSystem.Collections.Generic.List<KeyCode> k, Text t) =>
                    {
                        APIUser player = Splayer.field_Private_APIUser_0;

                        Il2CppSystem.String worldId = (Il2CppSystem.String)"";
                        Il2CppSystem.String worldName = (Il2CppSystem.String)$"\n[IceBurn]: {s}";

                        NotificationDetails notificationDetails = new NotificationDetails();
                        notificationDetails.Add("worldId", worldId.Cast<Il2CppSystem.Object>());
                        notificationDetails.Add("worldName", worldName.Cast<Il2CppSystem.Object>());
                        NotificationManager.prop_NotificationManager_0.Method_Public_Void_String_String_String_NotificationDetails_0(player.id, "invite", string.Empty, notificationDetails);
                        IceLogger.Log(string.Concat(new string[]
                        {
                            "Sent Invite Message to : ",
                            player.displayName,
                            " [With Message:",
                            s,
                            "]"
                        }));
                    }));
                }
                catch (Exception ex)
                {
                    Popup.Alert("Message Error!", $"Can't Send Message {Wrapper.GetQuickMenu().GetSelectedPlayer().ToString()}", "Close");
                    IceLogger.Error(ex.ToString());
                }
            }), "Send Message TO: Player");

            GetAvatars = new QMSingleButton(userUtilsMenu, 1, 1, "Get\nAvatars", new Action(() =>
            {
                IceLogger.Log(Wrapper.GetQuickMenu().GetSelectedPlayer().field_Internal_VRCPlayer_0.prop_String_1);
                List<avi> avatars = PlayerWrapper.GetPublicAvatars(Wrapper.GetQuickMenu().GetSelectedPlayer().field_Private_String_0);

                foreach (var item in avatars)
                    IceLogger.Log(item.AssetUrl);

            }), "Get Public Avatars Of This User");
        }
    }
}
