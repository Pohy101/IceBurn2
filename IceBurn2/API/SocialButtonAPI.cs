using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

namespace IceBurn.API
{
    public static class SocialButtonAPI
    {
        //REPLACE THIS STRING SO YOUR MENU DOESNT COLLIDE WITH OTHER MENUS
        public static string identifier = "IceBurn";
        public static Color mBackground = Color.red;
        public static Color mForeground = Color.white;
        public static Color bBackground = Color.red;
        public static Color bForeground = Color.yellow;
        public static List<SocialSingleButton> allSocialSingleButtons = new List<SocialSingleButton>();
    }

    public class SocialButtonBase
    {
        protected GameObject button;
        protected string btnQMLoc;
        protected string btnType;
        protected string btnTag;
        protected Color OrigBackground;
        protected Color OrigText;

        public void setLocation(float buttonXLoc, float buttonYLoc)
        {
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.right * buttonXLoc;
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.down * buttonYLoc;
            //button.transform.localPosition = new Vector3(buttonXLoc, buttonYLoc + 150f);

            btnTag = "(" + buttonXLoc + "," + buttonYLoc + ")";
            button.name = btnQMLoc + "/" + btnType + btnTag;
            button.GetComponent<Button>().name = btnType + btnTag;
        }

        public void setButtonText(string buttonText)
        {
            button.GetComponentInChildren<Text>().text = buttonText;
        }

        public void setAction(System.Action buttonAction)
        {
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            if (buttonAction != null)
                button.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(buttonAction));
        }

        public void setActive(bool isActive)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    public class SocialSingleButton : SocialButtonBase
    {
        public SocialSingleButton(float btnXLocation, float btnYLocation, String btnText, System.Action btnAction, Color? btnBackgroundColor = null, Color? btnTextColor = null)
        {
            initButton(btnXLocation, btnYLocation, btnText, btnAction, btnBackgroundColor, btnTextColor);
        }

        private void initButton(float btnXLocation, float btnYLocation, String btnText, System.Action btnAction, Color? btnBackgroundColor = null, Color? btnTextColor = null)
        {
            GameObject screens = GameObject.Find("Screens");
            button = UnityEngine.Object.Instantiate(screens.transform.Find("UserInfo/User Panel/Playlists/PlaylistsButton").gameObject, screens.transform.Find("UserInfo/User Panel/Playlists/PlaylistsButton").transform.parent);

            setLocation(btnXLocation, btnYLocation);
            setButtonText(btnText);
            setAction(btnAction);


            if (btnBackgroundColor != null)
                setBackgroundColor((Color)btnBackgroundColor);
            else
                OrigBackground = button.GetComponentInChildren<UnityEngine.UI.Image>().color;

            if (btnTextColor != null)
                setTextColor((Color)btnTextColor);
            else
                OrigText = button.GetComponentInChildren<Text>().color;

            setActive(true);
            SocialButtonAPI.allSocialSingleButtons.Add(this);
        }

        public void setBackgroundColor(Color buttonBackgroundColor, bool save = true)
        {
            //button.GetComponentInChildren<UnityEngine.UI.Image>().color = buttonBackgroundColor;
            if (save)
                OrigBackground = (Color)buttonBackgroundColor;
            //UnityEngine.UI.Image[] btnBgColorList = ((btnOn.GetComponentsInChildren<UnityEngine.UI.Image>()).Concat(btnOff.GetComponentsInChildren<UnityEngine.UI.Image>()).ToArray()).Concat(button.GetComponentsInChildren<UnityEngine.UI.Image>()).ToArray();
            //foreach (UnityEngine.UI.Image btnBackground in btnBgColorList) btnBackground.color = buttonBackgroundColor;
            button.GetComponentInChildren<UnityEngine.UI.Button>().colors = new ColorBlock()
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = buttonBackgroundColor * 1.5f,
                normalColor = buttonBackgroundColor / 1.5f,
                pressedColor = Color.grey * 1.5f
            };
        }
        public void setTextColor(Color buttonTextColor, bool save = true)
        {
            button.GetComponentInChildren<Text>().color = buttonTextColor;
            if (save)
                OrigText = (Color)buttonTextColor;
        }
    }
}
