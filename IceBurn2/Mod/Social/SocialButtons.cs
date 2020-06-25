using IceBurn.API;
using IceBurn.Other;
using IceBurn.Utils;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;

namespace IceBurn.Mod.Social
{
    class SocialButtons : VRmod
    {
        public override string Name => "Avatar UI";
        public override string Description => "creates buttons in QM User Interface";

        public static SocialSingleButton cloneAvatarFromSocial;
        public static SocialSingleButton DropPortalToPlayer;

        public override void OnStart()
        {
            cloneAvatarFromSocial = new SocialSingleButton(-820f, 320f, "CloneAvatar", new Action(() =>
            {
                APIUser ourSelectedPlayer = PlayerWrapper.GetAPIUserFromSocialPage();
                try
                {
                    new PageAvatar { avatar = new SimpleAvatarPedestal { field_Internal_ApiAvatar_0 = new ApiAvatar { id = ourSelectedPlayer.avatarId } } }.ChangeToSelectedAvatar();
                    IceLogger.Log(ourSelectedPlayer.avatarId);
                }
                catch (Exception)
                {
                    IceLogger.Error("User not selected!");
                    throw;
                }
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
