using IceBurn.API;
using IceBurn.Other;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceBurn.Mod.Social
{
    class SocialButtons : VRmod
    {
        public override string Name => "Avatar UI";
        public override string Description => "creates buttons in QM User Interface";

        public static SocialSingleButton cloneAvatarFromSocial;

        public override void OnStart()
        {
            cloneAvatarFromSocial = new SocialSingleButton("cloneAvatarFromSocial", -20f, 0f, "CloneAvatar", new Action(() =>
            {
                IceLogger.Log("Clonning Avatar)");
            }));
        }
    }
}
