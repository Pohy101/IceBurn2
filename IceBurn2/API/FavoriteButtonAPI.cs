using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace IceBurn.API
{
    public static class FavButtonAPI
    {
        //REPLACE THIS STRING SO YOUR MENU DOESNT COLLIDE WITH OTHER MENUS
        public static string identifier = "IceBurn";
        public static Color mBackground = Color.red;
        public static Color mForeground = Color.white;
        public static Color bBackground = Color.red;
        public static Color bForeground = Color.yellow;
        public static List<FavSingleButton> allFavSingleButtons = new List<FavSingleButton>();
    }

    public class FavButtonBase
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

    public class FavSingleButton : FavButtonBase
    {
        public FavSingleButton(String btnName, float btnXLocation, float btnYLocation, String btnText, System.Action btnAction, Color? btnBackgroundColor = null, Color? btnTextColor = null)
        {
            btnQMLoc = btnName;
            initButton(btnName, btnXLocation, btnYLocation, btnText, btnAction, btnBackgroundColor, btnTextColor);
        }

        private void initButton(String btnName, float btnXLocation, float btnYLocation, String btnText, System.Action btnAction, Color? btnBackgroundColor = null, Color? btnTextColor = null)
        {
            button = UnityEngine.Object.Instantiate(GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Change Button"), GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Change Button").transform.parent);

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
            FavButtonAPI.allFavSingleButtons.Add(this);
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
    public class AvatarListApi
    {
        private static UiAvatarList aviList = null;
        public GameObject GameObj;
        public UiAvatarList AList;
        public Button ListBtn;
        public Text ListTitle;


        public static UiAvatarList AviList
        {
            get
            {
                if (aviList == null)
                {
                    var pageAvatar = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar");
                    var vlist = pageAvatar.transform.Find("Vertical Scroll View/Viewport/Content");
                    var updatethis = vlist.transform.Find("Favorite Avatar List").gameObject;
                    updatethis = GameObject.Instantiate(updatethis, updatethis.transform.parent);
                    var avText = updatethis.transform.Find("Button");                                   // I make a invis list because 1 doesn't activate or have anything 
                    avText.GetComponentInChildren<Text>().text = "New List";                            // running and its just easier to copy / less todo on copy.
                    var UpdateValue = updatethis.GetComponent<UiAvatarList>();
                    //UpdateValue.category = UiAvatarList.Nested0.SpecificList;
                    UpdateValue.category = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLi9vUnique.SpecificList;
                    UpdateValue.StopAllCoroutines();
                    updatethis.SetActive(false);
                    aviList = UpdateValue;
                }
                return aviList;
            }
        }

        public static AvatarListApi Create(string listname, int index)
        {
            var list = new AvatarListApi();
            list.GameObj = GameObject.Instantiate(AviList.gameObject, AviList.transform.parent);
            list.GameObj.transform.SetSiblingIndex(index);
            list.AList = list.GameObj.GetComponent<UiAvatarList>();
            list.ListBtn = list.AList.GetComponentInChildren<Button>();
            list.ListTitle = list.AList.GetComponentInChildren<Text>();
            list.ListTitle.text = listname;
            list.AList.hideWhenEmpty = true;
            list.AList.clearUnseenListOnCollapse = true;
            list.GameObj.SetActive(true);
            return list;
        }

        public void SetAction(Action v)
        {
            ListBtn.onClick = new Button.ButtonClickedEvent();
            ListBtn.onClick.AddListener(v);
        }
    }
}
