using IceBurn.API;
using IceBurn.Utils;
using IceModSystem;
using Logger;
using System;
using System.Collections.Generic;
using Transmtn.DTO.Notifications;
using UnityEngine;
using VRC;
using VRC.SDKBase;

namespace IceBurn.Mod
{
    public class FriendUtils: VRmod
    {
        public override int LoadOrder => 12;

        public static QMSingleButton FriendAll;//add all players in room to friends
        public static QMSingleButton FriendCheck;//check if there are friends in current room

        public override void OnStart()
        {
            FriendAll = new QMSingleButton(UI.mainMenuP3, 1, 0, "Friend All", new Action(() =>
            {
                try
                {
                    NotificationManager field_Private_Static_NotificationManager_ = NotificationManager.field_Private_Static_NotificationManager_0;
                    IceLogger.Log(" -----------------------------------Adding ALL Players-----------------------------------");
                    Player[] array = PlayerWrapper.GetAllPlayers().ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        Player player = array[i];
                        string id = player.field_Private_APIUser_0.id;
                        Notification notification = FriendRequest.Create(id);
                        if (notification == null || player == null)
                        {
                            break;
                        }

                        //check if not already on friendlist and not ourselves, if not add to friend
                        if (!player.field_Private_APIUser_0.isFriend && PlayerWrapper.GetCurrentPlayer().prop_String_1 != player.field_Internal_VRCPlayer_0.prop_String_1)
                        {
                            IceLogger.Log(" Adding ---> " + player.field_Private_APIUser_0.displayName + "    (User ID) = " + id);
                            field_Private_Static_NotificationManager_.Method_Public_Void_String_String_String_NotificationDetails_0(id, notification.type, "", null);
                        }
                        else
                        {
                            IceLogger.Log(string.Concat(new string[] {" Not Adding To Friend ---> [",player.field_Private_APIUser_0.displayName,"    (User ID) = ",id,"]"}));
                        }
                    }
                    
                    IceLogger.Log(" -----------------------------------Done Adding players--------------------------------");
                }

                catch (Exception ex)
                {
                    IceLogger.Error(" Friend all error, please report this to lepper#4437");
                    IceLogger.Error(ex.ToString());
                }
            }), "Add all players in current room to friends");

             FriendCheck = new QMSingleButton(UI.mainMenuP3, 2, 0, "Check for friends", new Action(() =>
            {
                try
                {
                    NotificationManager field_Private_Static_NotificationManager_ = NotificationManager.field_Private_Static_NotificationManager_0;
                    IceLogger.Log(" -----------------------------------Looking for friends-----------------------------------");
                    Player[] PlayersArray = PlayerWrapper.GetAllPlayers().ToArray();
                    int num = 0;
                    for (int i = 0; i < PlayersArray.Length; i++)
                    {
                        Player player = PlayersArray[i];
                        string id = player.field_Private_APIUser_0.id;
                        
                        //check if is on friendlist and not ourselves, if yes write nick
                        if (player.field_Private_APIUser_0.isFriend && PlayerWrapper.GetCurrentPlayer().prop_String_1 != player.field_Internal_VRCPlayer_0.prop_String_1)
                        {
                            IceLogger.Log(" Friend found ---> " + player.field_Private_APIUser_0.displayName + "    (User ID) = " + id);
                            num++;
                        }
                    }

                    if (num > 0)
                    {
                        IceLogger.Log("\n Number of friends found: " + num);
                    }
                    else
                    {
                        IceLogger.Log(" There are no friends currently in this room. Go and find some!");
                    }
                }

                catch (Exception ex)
                {
                    IceLogger.Error(" Friend check error, please report this to lepper#4437");
                    IceLogger.Error(ex.ToString());
                }
            }), "Check for friends in the room");
        }
    }
}
