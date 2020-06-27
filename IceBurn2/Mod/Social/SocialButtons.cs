using IceBurn.API;
using IceBurn.Other;
using IceBurn.Utils;
using System;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using Logger;

namespace IceBurn.Mod.Social
{
    class SocialButtons : VRmod
    {
        public static SocialSingleButton cloneAvatarFromSocial;
        public static SocialSingleButton DropPortalToPlayer;
        public static SocialSingleButton ForceJoinToPlayer;

        public override void OnStart()
        {
            cloneAvatarFromSocial = new SocialSingleButton(-820f, 400f, "CloneAvatar", new Action(() =>
            {
                /*APIUser ourSelectedPlayer = PlayerWrapper.GetAPIUserFromSocialPage();
                ApiAvatar avatar = PlayerWrapper.GetPlayer(ourSelectedPlayer.id).field_Internal_VRCPlayer_0.prop_ApiAvatar_0;

                if (avatar.releaseStatus != "private")
                    new PageAvatar { avatar = new SimpleAvatarPedestal { field_Internal_ApiAvatar_0 = new ApiAvatar { id = avatar.id } } }.ChangeToSelectedAvatar();
                else
                {
                    IceLogger.Log("Avatar release status is PRIVATE!");
                    Console.Beep();
                }
                IceLogger.Log(avatar.id);*/

                APIUser ourSelectedPlayer = PlayerWrapper.GetAPIUserFromSocialPage();
                IceLogger.Log("Avatar ID:" + ourSelectedPlayer.avatarId);
                IceLogger.Log("Blob:" + ourSelectedPlayer.blob);
                IceLogger.Log("Asset URL:" + ourSelectedPlayer.currentAvatarAssetUrl);
                IceLogger.Log("ThumbnailImage URL:" + ourSelectedPlayer.currentAvatarThumbnailImageUrl);
                IceLogger.Log("Image URL:" + ourSelectedPlayer.currentAvatarImageUrl);
            }));

            ForceJoinToPlayer = new SocialSingleButton(-1030, 400f, "Force Join", new Action(() =>
            {
                APIUser ourSelectedPlayer = PlayerWrapper.GetAPIUserFromSocialPage();
                if (ourSelectedPlayer.location != null && ourSelectedPlayer.location != "")
                    IceLogger.Log(ourSelectedPlayer.username + "'s Location: " + ourSelectedPlayer.location);
            }));

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
