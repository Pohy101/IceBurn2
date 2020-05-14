using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IceBurn.Utils
{
    public static class PlayerWrapper
    {
        public static VRCPlayer GetCurrentPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
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
    }
}
