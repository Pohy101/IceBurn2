using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;

namespace IceBurn.Utils
{
    public static class PlayerWrapper
    {
        public static VRCPlayer GetCurrentPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }

        public static Il2CppSystem.Collections.Generic.List<Player> GetAllPlayers(this PlayerManager instance)
        {
            return instance.field_Private_List_1_Player_0;
        }

        public static APIUser GetAPIUser(this Player player)
        {
            return player.field_Private_APIUser_0;
        }

        public static Player GetPlayer(this PlayerManager instance, string UserID)
        {
            Il2CppSystem.Collections.Generic.List<Player> Players = instance.GetAllPlayers();
            Player Foundplayer = null;
            for(int i = 0; i < Players.Count; i++)
            {
                Player player = Players[i];
                if (player.GetAPIUser().id == UserID)
                {
                    Foundplayer = player;
                }
            }
            return Foundplayer;
        }
    }

    public static class Wrapper
    {
        public static GameObject GetPlayerCamera()
        {
            return GameObject.Find("Camera (eye)");
        }

        public static void EnableOutline(this HighlightsFX instance, Renderer renderer, bool state)
        {
            instance.Method_Public_Void_Renderer_Boolean_0(renderer, state); //First method to take renderer, bool parameters
        }

        public static HighlightsFX GetHighlightsFX()
        {
            return HighlightsFX.prop_HighlightsFX_0;
        }

        public static PlayerManager GetPlayerManager()
        {
            return PlayerManager.prop_PlayerManager_0;
        }

        public static QuickMenu GetQuickMenu()
        {
            return QuickMenu.prop_QuickMenu_0;
        }

        public static Player GetSelectedPlayer(this QuickMenu instance)
        {
            APIUser APIUser = instance.field_Private_APIUser_0;
            PlayerManager playerManager = Wrapper.GetPlayerManager();
            return playerManager.GetPlayer(APIUser.id);
        }
    }
}
