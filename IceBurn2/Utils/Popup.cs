using System;
using UnityEngine;
using UnityEngine.UI;

namespace IceBurn.Utils.Popup
{
    public static class Popup
    {
        public static void Alert(string title, string Content, string mainBtn = "Main Button", Action mainAC = null)
        {
            if (mainAC == null)
                mainAC = new Action(() => { HideCurrentPopUp(); });

            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_2(title, Content, mainBtn, mainAC, null);
        }

        public static void Alert2(string title, string Content, string leftBtn = "Left Button", Action leftAC = null, string rightBtn = "Right Button", Action rightAC = null)
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, Content, leftBtn, leftAC, rightBtn, rightAC, null);
        }

        public static void ShowUnityInputPopupWithCancel(string title, string TextInField, string RightButtonText, Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text> action)
        {
            VRCUiPopupManager.prop_VRCUiPopupManager_0.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_0(title, TextInField, InputField.InputType.Standard, false, RightButtonText, action, new Action(() => { }));
        }

        public static void HideCurrentPopUp()
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_12();
            //VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_10(); WTF????
            //VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_9(); with ERROR x3
            //VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_6(); with ERROR x2
            //VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_5(); with performance options
            //VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_3(); with ERROR
            //VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_2(); with download data
        }
    }
}
