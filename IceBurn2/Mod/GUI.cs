using IceBurn.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace IceBurn.Mod
{
    class GUI : VRmod
    {
        public override string Name => "User Interface";
        public override string Description => "To see elements on screen";

        /*public override void OnStart()
        {
            GameObject myRoot;
            GameObject myText;
            //Canvas myCanvas;
            Text text;
            RectTransform rectTransform;

            myRoot = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(myRoot);
            myRoot.name = "TestCanvas";

            myCanvas = myRoot.GetOrAddComponent<Canvas>();
            myRoot.GetOrAddComponent<CanvasScaler>();
            myRoot.GetOrAddComponent<GraphicRaycaster>();//.ignoreReversedGraphics = true;
            myCanvas.sortingOrder = 32767;
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Text
            myText = new GameObject();
            myText.transform.parent = myGO.transform;
            myText.name = "wibble";

            text = myText.AddComponent<Text>();
            text.font = (Font)Resources.Load("Arial");
            text.text = (Time.frameCount / Time.time).ToString();
            text.fontSize = 100;

            // Text position
            rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(400, 200);
        }*/

        public override void OnUpdate()
        {

        }
    }
}
