using IceBurn.Utils;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IceBurn.API
{
    // Создаем основу кнопки от Quick Menu
    public class QMButtonBase
    {
        protected GameObject button;
        protected string btnQMLoc;
        protected string btnType;
        protected string btnTag;
        protected int[] position = { 0, 0 };

        public GameObject GetGameObject()
        {
            return button;
        }

        public void setActive(bool isActive)
        {
            button.gameObject.SetActive(isActive);
        }

        public void setLocation(int btnXLocation, int btnYLocation)
        {
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (420 * (btnXLocation + position[0]));
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.down * (420 * (btnYLocation + position[1]));

            btnTag = "(" + btnXLocation + ", " + btnYLocation + ")";
            button.name = btnQMLoc + "/" + btnType + btnTag;
            button.GetComponent<Button>().name = btnType + btnTag;
        }

        public void setToolTip(string btnToolTip)
        {
            button.GetComponent<UiTooltip>().text = btnToolTip;
            button.GetComponent<UiTooltip>().alternateText = btnToolTip;
        }
    }

    // ! Потом нужно добавить QMNested версию
    public class QMSingleButton : QMButtonBase
    {
        // ! Nesded версию сюда

        public QMSingleButton(string btnMenu, int btnXLocation, int btnYLocation, String btnText, UnityAction btnAction, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            btnQMLoc = btnMenu;
            btnType = "SingleButton";
            // ! Инициализация
        }

        private void initButton(int btnXlocation, int btnYLocation, String btnText, UnityAction btnAction, String btnToolTip, Nullable<Color> btnBackgroundColor = null, Nullable<Color> btnTextColor = null)
        {
            Transform btnTemplate = null;
            // ! btnTemplate = Qui
        }
    }

    public class QuickMenuStuff : MonoBehaviour
    {
        private static VRCUiManager vrcuimInstance = Wrapper.GetRCUiManager();

        // Чтобы быстрее достать QM
        public static QuickMenu GetQuickMenuInstance()
        {
            return Wrapper.GetQuickMenu();
        }

        // System Reflection
        private static FieldInfo currentPageGetter;
        private static FieldInfo quickmenuContextDisplayGetter; // что?

        public static void ShowQuickmenuPage(string pagename)
        {
            QuickMenu quickMenuInstance = GetQuickMenuInstance();
            Transform transform = (quickMenuInstance != null ? quickMenuInstance.transform.Find(pagename) : null);
            if (transform == null)
            {
                IceLogger.Error("[QuickMenuUtils] pageTransform is null !");
            }
            if (currentPageGetter == null)
            {
                GameObject SCMenu = quickMenuInstance.transform.Find("ShortcutMenu").gameObject;
                FieldInfo[] fieldInfosSC = (from fi in typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.Public) where fi.FieldType == typeof(GameObject) select fi).ToArray<FieldInfo>(); // ебать какая длинная строка
                int numSC = 0;
                foreach (FieldInfo fieldInfo in fieldInfosSC)
                {
                    GameObject go = fieldInfo.GetValue(quickMenuInstance) as GameObject;
                    IceLogger.Log(go.name);

                    if (fieldInfo.GetValue(quickMenuInstance) as GameObject == SCMenu && ++numSC == 2)
                    {
                        currentPageGetter = fieldInfo;
                        break;
                    }
                }

                GameObject UserInteractMenu = quickMenuInstance.transform.Find("UserInteractMenu").gameObject;
                FieldInfo[] fieldInfosUInteract = (from fi in typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.Public) where fi.FieldType == typeof(GameObject) select fi).ToArray<FieldInfo>();
                int numUInteract = 0;
                foreach (FieldInfo fieldInfo in fieldInfosUInteract)
                {
                    if (fieldInfo.GetValue(quickMenuInstance) as GameObject == UserInteractMenu && ++numUInteract == 2)
                    {
                        currentPageGetter = fieldInfo;
                        break;
                    }
                }
                IceLogger.Error("[QuickMenuUtils] Unable to find field currentPage in QuickMenu");
                return;
            }

            GameObject obj = (GameObject)currentPageGetter.GetValue(quickMenuInstance);
            if (obj != null)
                obj.SetActive(false);
            GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar").gameObject.SetActive(false);

            if (quickmenuContextDisplayGetter != null)
                quickmenuContextDisplayGetter = typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault((FieldInfo fi) => fi.FieldType == typeof(QuickMenuContextualDisplay));

            FieldInfo fieldInfo2 = quickmenuContextDisplayGetter;
            QuickMenuContextualDisplay quickMenuContextualDisplay = ((fieldInfo2 != null) ? fieldInfo2.GetValue(quickMenuInstance) : null) as QuickMenuContextualDisplay;
            if (quickMenuContextualDisplay != null)
            {
                currentPageGetter.SetValue(quickMenuInstance, transform.gameObject);
                quickMenuContextualDisplay.Method_Public_Void_EnumNPublicSealedvaUnNoToUs7vUsNoUnique_0(0);
            }
            currentPageGetter.SetValue(quickMenuInstance, transform.gameObject);
            quickMenuContextualDisplay.Method_Public_Void_EnumNPublicSealedvaUnNoToUs7vUsNoUnique_0(0);
            transform.gameObject.SetActive(true);
        }
    }
}
