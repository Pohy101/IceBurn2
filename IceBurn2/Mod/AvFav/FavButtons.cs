using IceBurn.API;
using IceBurn.Mods.Fav.Config;
using IceBurn.Other;
using System;
using System.Diagnostics;
using System.Linq;
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

        public static AvatarListApi CustomList;

        public override void OnStart()
        {
            CustomList = AvatarListApi.Create(Config.CFG.CustomName + " / " + Config.DAvatars.Count, 1);
            CustomList.AList.FirstLoad(Config.DAvatars);

            Il2CppSystem.Delegate test = (Il2CppSystem.Action<string, GameObject, VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats>)new Action<string, GameObject, VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats>((x, y, z) =>
            {
                if (Config.DAvatars.Any(v => v.AvatarID == CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0.id))
                {
                    AddRemoveFavorite.setButtonText(Config.CFG.RemoveFavoriteTXT);
                    CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                }
                else
                {
                    AddRemoveFavorite.setButtonText(Config.CFG.AddFavoriteTXT);
                    CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                }
            });

            CustomList.AList.avatarPedestal.field_Internal_Action_3_String_GameObject_AvatarPerformanceStats_0 = Il2CppSystem.Delegate.Combine(CustomList.AList.avatarPedestal.field_Internal_Action_3_String_GameObject_AvatarPerformanceStats_0, test).Cast<Il2CppSystem.Action<string, GameObject, VRCSDK2.Validation.Performance.Stats.AvatarPerformanceStats>>();

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

            AddRemoveFavorite = new FavSingleButton("AddRemoveFavorite", 0f, -80f, Config.CFG.AddFavoriteTXT, new Action(() =>
            {
                var avatar = CustomList.AList.avatarPedestal.field_Internal_ApiAvatar_0;
                if (avatar.releaseStatus != "private")
                {
                    if (!Config.DAvatars.Any(v => v.AvatarID == avatar.id))
                    {
                        AvatarListHelper.AvatarListPassthru(avatar);
                        CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                        AddRemoveFavorite.setButtonText(Config.CFG.AddFavoriteTXT);
                        CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                    }
                    else
                    {

                        AvatarListHelper.AvatarListPassthru(avatar);
                        CustomList.AList.Refresh(Config.DAvatars.Select(x => x.AvatarID).Reverse());
                        AddRemoveFavorite.setButtonText(Config.CFG.AddFavoriteTXT);
                        CustomList.ListTitle.text = Config.CFG.CustomName + " / " + Config.DAvatars.Count;
                    }
                }
            }));
        }

        public override void OnUpdate()
        {

        }
    }
}
