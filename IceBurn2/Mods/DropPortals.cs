using IceBurn.API;
using IceBurn.Utils;
using IceModSystem;
using Logger;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.SDKBase;

namespace IceBurn.Mod
{
    public class DropPortals : VRmod
    {
        public override int LoadOrder => 10;

        public static QMNestedButton dropPortalMenu;
        private static List<QMHalfButton> tPlayerList = new List<QMHalfButton>();
        private static List<Player> tmpPlayerList = new List<Player>();

        public override void OnStart()
        {
            dropPortalMenu = new QMNestedButton(UI.mainMenuP2, 4, 1, "Drop\nPortal", new Action(() =>
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
                                        (Il2CppSystem.String)"wrld_3765d091-e420-4e2f-ae63-0dcad48cf5f5",
                                        //(Il2CppSystem.String)Clipboard.GetText(),
                                        (Il2CppSystem.String)$" {player.GetAPIUser().displayName} \0",
                                        new Il2CppSystem.Int32
                                        {
                                            m_value = -2137
                                        }.BoxIl2CppObject()
                                    });
                                }
                                catch (Exception ex)
                                {
                                    IceLogger.Error(ex.ToString());
                                }
                        }), "Drop Portal TO: " + player.ToString());

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
                            tmpButton.setAction(null);
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
                                        (Il2CppSystem.String)"wrld_3765d091-e420-4e2f-ae63-0dcad48cf5f5",
                                        (Il2CppSystem.String)$" {player.GetAPIUser().displayName} \0",
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
                        }), "Drop Portal TO: " + player.ToString());

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
                            tmpButton.setAction(null);
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
            }), "Drop Portal TO Player");
        }
    }
}
