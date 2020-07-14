using IceBurn.API;
using IceBurn.Utils;
using IceModSystem;
using Logger;
using System;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace IceBurn.Mod
{
    class SocialButtons : VRmod
    {
        public override int LoadOrder => 10;

        public static SocialSingleButton DropPortalToPlayer;

        public override void OnStart()
        {
            DropPortalToPlayer = new SocialSingleButton(0, -76.0f, "Drop portal To", new Action(() =>
            {
                APIUser ourSelectedPlayer = PlayerWrapper.GetAPIUserFromSocialPage();
                if (ourSelectedPlayer.location != null && ourSelectedPlayer.location != "")
                {
                    VRCPlayer player = PlayerWrapper.GetCurrentPlayer();
                    GameObject portal = Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", player.transform.position + player.transform.forward * 1.5f, player.transform.rotation);
                    string[] location = ourSelectedPlayer.location.Split(new char[]
                    {
                        ':'
                    });
                    IceLogger.Log("Dropping portal to player in world ID " + location[0] + " with instance ID " + location[1]);
                    Networking.RPC(RPC.Destination.AllBufferOne, portal, "ConfigurePortal", new Il2CppSystem.Object[]
                    {
                        (Il2CppSystem.String)location[0],
                        (Il2CppSystem.String)location[1],
                        new Il2CppSystem.Int32
                        {
                            m_value = 0
                        }.BoxIl2CppObject()
                    });
                    VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_2(false);
                }
            }));
        }
    }
}
