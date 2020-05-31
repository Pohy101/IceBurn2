using System;
using IceBurn;
using VRC.Core;
using MelonLoader;
using UnityEngine;

namespace DiscordRichPresence
{
    internal static class DiscordManager
    {
        private static DiscordRpc.RichPresence presence;
        private static DiscordRpc.EventHandlers eventHandlers;
        private static bool running = false;
        private static string roomId = "";
        private static string roomSecret = "";
        private static int playersInRoom = 0;
        public static void Init()
        {
            DiscordManager.eventHandlers = default(DiscordRpc.EventHandlers);
            DiscordManager.eventHandlers.errorCallback = delegate (int code, string message)
            {
                IceLogger.Error(string.Concat(new object[]
                {
                    "[DiscordRichPresence] (E",
                    code,
                    ") ",
                    message
                }));
            };
            DiscordManager.presence.state = "Loading world...";
            DiscordManager.presence.details = "Logged in (" + (VRCTrackingManager.Method_Public_Static_Boolean_11() ? "VR" : "PC") + ")";
            DiscordManager.presence.largeImageKey = "logo";
            DiscordManager.presence.partySize = 0;
            DiscordManager.presence.partyMax = 0;
            DiscordManager.presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            DiscordManager.presence.partyId = "";
            DiscordManager.presence.largeImageText = "VRChat";
            DiscordManager.DeviceChanged();
            try
            {
                string optionalSteamId = null;
                ApiServerEnvironment serverEnvironment = VRCApplicationSetup.prop_VRCApplicationSetup_0.ServerEnvironment;
                if (serverEnvironment == ApiServerEnvironment.Release)
                {
                    optionalSteamId = "438100";
                    DiscordManager.presence.largeImageText = DiscordManager.presence.largeImageText + " 2";
                }
                DiscordRpc.Initialize("711923739188527124", ref DiscordManager.eventHandlers, true, optionalSteamId);
                DiscordRpc.UpdatePresence(ref DiscordManager.presence);
                DiscordManager.running = true;
                IceLogger.Log("[DiscordManager] RichPresence Initialised");
            }
            catch (Exception arg)
            {
                IceLogger.Error("[DiscordManager] Unable to init discord RichPresence:");
                IceLogger.Error("[DiscordManager] " + arg);
            }
        }


        public static void DeviceChanged()
        {
            bool flag = VRCTrackingManager.Method_Public_Static_Boolean_11();
            //string model = XRDevice.model;
            if (flag) // XRDevice was stripped from VRChat as of the IL2CPP update
            {
                /*if (model.ToLower().Contains("oculus") || model.ToLower().Contains("rift"))
				{
					DiscordManager.presence.smallImageKey = "headset_rift";
					DiscordManager.presence.smallImageText = "Oculus Rift";
				}
				else if (model.ToLower().Contains("htc") || model.ToLower().Contains("vive"))
				{
					DiscordManager.presence.smallImageKey = "headset_vive";
					DiscordManager.presence.smallImageText = "HTC Vive";
				}
				else*/
                {
                    DiscordManager.presence.smallImageKey = "headset_generic";
                    DiscordManager.presence.smallImageText = "VR Headset";
                }
            }
            else
            {
                DiscordManager.presence.smallImageKey = "desktop";
                DiscordManager.presence.smallImageText = "Desktop";
            }
            //IceLogger.Log("[DiscordManager.DeviceChanged] isInVR: " + flag.ToString());
        }


        public static string RoomChanged(string worldName, string worldAndRoomId, string roomIdWithTags, ApiWorldInstance.AccessType accessType, int maxPlayers)
        {
            if (!DiscordManager.running)
            {
                return null;
            }
            if (!worldAndRoomId.Equals(""))
            {
                if (accessType == ApiWorldInstance.AccessType.InviteOnly || accessType == ApiWorldInstance.AccessType.InvitePlus)
                {
                    DiscordManager.presence.state = "In a private world";
                    DiscordManager.presence.partyId = "";
                    DiscordManager.presence.startTimestamp = 0L;
                }
                else
                {
                    string str = " [Unknown]";
                    if (accessType == ApiWorldInstance.AccessType.FriendsOfGuests)
                    {
                        str = " [Friends+]";
                    }
                    else if (accessType == ApiWorldInstance.AccessType.FriendsOnly)
                    {
                        str = " [Friends]";
                    }
                    else if (accessType == ApiWorldInstance.AccessType.Public)
                    {
                        str = " [Public]";
                    }
                    else
                    {
                        str = "";
                    }
                    DiscordManager.presence.state = "in " + worldName + str;
                    DiscordManager.presence.partyId = worldAndRoomId;
                    DiscordManager.presence.partyMax = maxPlayers;
                    DiscordManager.presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                }
            }
            else
            {
                DiscordManager.presence.state = "Loading world...";
                DiscordManager.presence.partyId = "";
                DiscordManager.presence.partyMax = 0;
                DiscordManager.presence.startTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                DiscordManager.presence.joinSecret = "";
            }
            DiscordRpc.UpdatePresence(ref DiscordManager.presence);
            return DiscordManager.presence.joinSecret;
        }


        public static void UserChanged(string displayName)
        {
            if (!DiscordManager.running)
            {
                return;
            }
            if (!displayName.Equals(""))
            {
                DiscordManager.presence.details = "Logged in (" + (VRCTrackingManager.Method_Public_Static_Boolean_11() ? "VR" : "PC") + ")";
                /*else
                {
                    DiscordManager.presence.details = string.Concat(new string[]
                    {
                        "as ",
                        displayName,
                        " (",
                        VRCTrackingManager.Method_Public_Static_Boolean_11() ? "VR" : "PC",
                        ")"
                    });
                }*/
                DiscordRpc.UpdatePresence(ref DiscordManager.presence);
                return;
            }
            DiscordManager.RoomChanged("", "", "", 0, 0);
        }


        public static void UserCountChanged(int usercount)
        {
            if (!DiscordManager.running)
            {
                return;
            }
            DiscordManager.presence.partySize = usercount;
            DiscordRpc.UpdatePresence(ref DiscordManager.presence);
        }


        public static void Update()
        {
            if (!DiscordManager.running)
            {
                return;
            }
            APIUser currentUser = APIUser.CurrentUser;
            string b = ((currentUser != null) ? currentUser.id : null) ?? "";
            /*if (DiscordManager.uuid != b || ModPrefs.GetBool("vrcdiscordpresence", "hidenameondiscord"))
            {
                DiscordManager.uuid = b;
                APIUser currentUser2 = APIUser.CurrentUser;
                DiscordManager.UserChanged(((currentUser2 != null) ? currentUser2.displayName : null) ?? "");
            }*/
            string text = "";
            ApiWorld currentRoom = RoomManagerBase.field_Internal_Static_ApiWorld_0;
            if (((currentRoom != null) ? currentRoom.currentInstanceIdOnly : null) != null)
            {
                text = currentRoom.id + ":" + currentRoom.currentInstanceIdWithTags;
            }
            if (DiscordManager.roomId != text)
            {
                DiscordManager.roomId = text;
                DiscordManager.roomSecret = "";
                if (DiscordManager.roomId != "")
                {
                    DiscordManager.roomSecret = DiscordManager.RoomChanged(currentRoom.name, currentRoom.id + ":" + currentRoom.currentInstanceIdOnly, currentRoom.currentInstanceIdWithTags, currentRoom.currentInstanceAccess, currentRoom.capacity);
                }
                else
                {
                    DiscordManager.RoomChanged("", "", "", ApiWorldInstance.AccessType.InviteOnly, 0);
                }
            }
            if (currentRoom != null)
            {
                int num = currentRoom.users.Count;
                if (text != "" && DiscordManager.playersInRoom != num)
                {
                    DiscordManager.playersInRoom = num;
                    DiscordManager.UserCountChanged(num);
                }
            }
        }


        public static void OnApplicationQuit()
        {
            if (!DiscordManager.running)
            {
                return;
            }
            DiscordRpc.Shutdown();
        }


        public static string GenerateRandomString(int length)
        {
            string text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] array = new char[length];
            System.Random random = new System.Random();
            for (int i = 0; i < length; i++)
            {
                array[i] = text[random.Next(text.Length)];
            }
            return new string(array);
        }



    }
}