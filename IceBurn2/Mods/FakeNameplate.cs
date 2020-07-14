using IceBurn.API;
using IceBurn.Utils;
using IceModSystem;
using System;
using System.Linq;
using UnityEngine;

namespace IceBurn.Mod
{
    public class FakeNameplate : VRmod
    {
        public override int LoadOrder => 14;

        public static QMToggleButton toggleFakeNamePlate;

        public override void OnStart()
        {
            toggleFakeNamePlate = new QMToggleButton(UI.mainMenuP1, 4, 1, "Fake Nameplate", new Action(() =>
            {
                var allPlayers = PlayerWrapper.GetAllPlayers();
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.red);
                    allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.red;
                    allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.red;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.red;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.clear;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlateTalkSprite = allPlayers[i].field_Internal_VRCPlayer_0.namePlateSilentSprite;
                }
            }), "Real Nameplate", new Action(() =>
            {
                PlayerWrapper.UpdateFriendList();

                var allPlayers = PlayerWrapper.GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    Transform sRegion = allPlayers[i].transform.Find("SelectRegion");
                    allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.green;
                    allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.white;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.white;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.black;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlateTalkSprite = allPlayers[i].field_Internal_VRCPlayer_0.namePlateSilentSprite;

                    if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Veteran user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.red);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Trusted user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.magenta);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Known user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.Lerp(Color.yellow, Color.red, 0.5f));
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "User")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.green);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "New user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(new Color(0.3f, 0.72f, 1f));
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Visitor")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.gray);

                    if (sRegion != null)
                        sRegion.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);

                    HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", Color.red);

                    if (allPlayers[i].field_Internal_VRCPlayer_0.prop_String_1 == "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223")
                    {
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.black);
                        allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.red;
                        allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.clear;
                        allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.red;
                        allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.red;
                    }
                }
            }), "Toggle Fake NameSpace");
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                PlayerWrapper.UpdateFriendList();

                var allPlayers = PlayerWrapper.GetAllPlayers().ToArray();
                for (int i = 0; i < allPlayers.Length; i++)
                {
                    Transform sRegion = allPlayers[i].transform.Find("SelectRegion");
                    allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.green;
                    allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.white;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.white;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.black;
                    allPlayers[i].field_Internal_VRCPlayer_0.namePlateTalkSprite = allPlayers[i].field_Internal_VRCPlayer_0.namePlateSilentSprite;

                    if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Veteran user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.red);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Trusted user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.magenta);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Known user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.Lerp(Color.yellow, Color.red, 0.5f));
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "User")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.green);
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "New user")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(new Color(0.3f, 0.72f, 1f));
                    else if (PlayerWrapper.GetTrustLevel(allPlayers[i]) == "Visitor")
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.gray);

                    if (sRegion != null)
                        sRegion.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);

                    HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", Color.red);

                    if (allPlayers[i].field_Internal_VRCPlayer_0.prop_String_1 == "usr_77979962-76e0-4b27-8ab7-ffa0cda9e223")
                    {
                        allPlayers[i].field_Private_VRCPlayerApi_0.SetNamePlateColor(Color.black);
                        allPlayers[i].field_Internal_VRCPlayer_0.namePlate.mainText.color = Color.red;
                        allPlayers[i].field_Internal_VRCPlayer_0.namePlate.dropShadow.color = Color.clear;
                        allPlayers[i].field_Internal_VRCPlayer_0.friendSprite.color = Color.red;
                        allPlayers[i].field_Internal_VRCPlayer_0.speakingSprite.color = Color.red;
                    }
                }
            }
        }
    }
}
