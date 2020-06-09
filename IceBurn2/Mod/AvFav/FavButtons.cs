using IceBurn.API;
using IceBurn.Other;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;

namespace IceBurn.Mod.AvFav
{
    class FavButtons : VRmod
    {
        public override string Name => "Avatar UI";
        public override string Description => "creates buttons in QM User Interface";
        public static FavSingleButton AddRemoveFavorite;
        public static FavSingleButton ShowAuthor;
        public static FavSingleButton FavDownloadVRCA;

        public override void OnStart()
        {
            AddRemoveFavorite = new FavSingleButton("AddRemoveFavorite", 0f, -80f, "Add / Remove", new Action(() =>
            {
                IceLogger.Log("It IS WORKING!");
            }));

            ShowAuthor = new FavSingleButton("ShowAuthor", 0f, -684f, "Show Author", new Action(() =>
            {
                VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_0(true);
                APIUser.FetchUser(AvatarListApi.AviList.avatarPedestal.field_Internal_ApiAvatar_0.authorId, new Action<APIUser>(x =>
                {
                    QuickMenu.prop_QuickMenu_0.prop_APIUser_0 = x;
                    QuickMenu.prop_QuickMenu_0.Method_Public_Void_Int32_Boolean_0(4, false);
                }), null);
            }));

            FavDownloadVRCA = new FavSingleButton("FavDownloadVRCA", 1150f, -684f, "Download VRCA", new Action(() =>
            {
                Process.Start(AvatarListApi.AviList.avatarPedestal.field_Internal_ApiAvatar_0.assetUrl);
            }));
        }

        public override void OnUpdate()
        {

        }
    }
}
