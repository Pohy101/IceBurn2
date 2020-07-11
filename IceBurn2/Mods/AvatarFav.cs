using IceBurn.API;
using IceModSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;

namespace IceBurn.Mod
{
    public class AvatarFav : VRmod
    {
        //public override int LoadOrder => 8;

        public static FavSingleButton FavoriteBtn;

        public override void OnStart()
        {
            //AvatarListAPI.setup(0, "[IceBurn] Favorite List");

            FavoriteBtn = new FavSingleButton("Favorite", 0f, -100f, "Favorite", delegate
            {
                /*ApiAvatar avatar = AvatarListAPI.listing_avatars.avatarPedestal.field_Internal_ApiAvatar_0;
                if (avatar.releaseStatus == null || AvatarListAPI.listing_avatars.avatarPedestal.field_Internal_ApiAvatar_0 == null || (avatar.releaseStatus == "private" && avatar.authorId != APIUser.CurrentUser.id))
                {

                    AvatarConfig.AvatarList.RemoveAll((AvatarStruct x) => x.AvatarID == avatar.id);
                    AvatarUtils.update_list(from x in AvatarConfig.AvatarList select x.AvatarID, AvatarListAPI.listing_avatars);
                }
                if (avatar.releaseStatus == "public" || avatar.authorId == APIUser.CurrentUser.id)
                {
                    if (!AvatarConfig.AvatarList.Any((AvatarStruct v) => v.AvatarID == avatar.id))
                    {
                        AvatarUtils.add_to_list(avatar);
                        AvatarUtils.update_list(from x in AvatarConfig.AvatarList select x.AvatarID, AvatarListAPI.listing_avatars);
                        FavoriteBtn.setButtonText($"Favorite {AvatarConfig.AvatarList.Count}");
                        return;
                    }
                    AvatarUtils.add_to_list(avatar);
                    AvatarUtils.update_list(from x in AvatarConfig.AvatarList select x.AvatarID, AvatarListAPI.listing_avatars);
                    FavoriteBtn.setButtonText($"UnFavorite {AvatarConfig.AvatarList.Count}");
                }*/
            });
            //AvatarListAPI.ui_avatar_list.avatarPedestal = new SimpleAvatarPedestal { field_Internal_ApiAvatar_0 = new ApiAvatar { id = "avtr_f3ee9d43-c019-4648-bd93-35f0bcfec07b" } };
        }

        public override void OnUpdate()
        {

        }
    }
}
