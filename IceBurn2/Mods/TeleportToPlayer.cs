using System;
using System.Collections.Generic;
using IceBurn.API;
using IceBurn.Utils;
using IceModSystem;
using Logger;
using UnityEngine;
using VRC;

namespace IceBurn.Mod
{
    public class TeleportToPlayer : VRmod
    {
        public override int LoadOrder => 12;

        public static QMNestedButton teleportMenu;
        private static List<QMHalfButton> tPlayerList = new List<QMHalfButton>();
        private static List<Player> tmpPlayerList = new List<Player>();

        public override void OnStart()
        {
            teleportMenu = new QMNestedButton(UI.mainMenuP1, 2, 0, "Teleport", new Action(() =>
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
        }
    }
}
