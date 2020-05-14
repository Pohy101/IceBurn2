using IceBurn.Other;
using static IceBurn.IceLog;
using IceBurn.Utils;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.SDKBase;

namespace IceBurn.Mod
{
    class InputHandler : VRmod
    {
        public override string Name => "Test Mod";
        public override string Description => "Testing first mod";

        // Переменные
        public static float flyspeed = 5f;

        // Инициализация
        public InputHandler() : base()
        {

        }

        public override void OnStart()
        {

        }

        public override void OnUpdate()
        {
            // Включает или выключает полёт при нажатий на F
            if (Input.GetKeyDown(KeyCode.F))
            {
                GlobalUtils.Fly = !GlobalUtils.Fly;
                Physics.gravity = GlobalUtils.Fly ? Vector3.zero : GlobalUtils.Gravity;
                if (GlobalUtils.Fly)
                    GlobalUtils.ToggleColliders(false);
                else
                    GlobalUtils.ToggleColliders(true);
                IceLogger($"Fly has been {(GlobalUtils.Fly ? "Enabled" : "Disabled")}.");
            }

            // Телепорт в точку которая находится на центре экрана по кнопку T
            if (Input.GetKeyDown(KeyCode.T))
                GlobalUtils.RayTeleport();

            // ESP или видеть игроков сквазь стены на кнопку O
            if (Input.GetKeyDown(KeyCode.O))
            {
                GlobalUtils.ESP = !GlobalUtils.ESP;
                IceLogger($"ESP has been {(GlobalUtils.Fly ? "Enabled" : "Disabled")}.");

                // Поиск и добавление игроков в ESP
                GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].transform.Find("SelectRegion"))
                    {
                        array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.ESP);
                    }
                }

                // Поиск и добавление обьектов в ESP    И СУКА ПОЧЕМУ ТО ОНО НЕ РАБОТАЕТ!
                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);
                        Wrapper.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.ESP);
                    }
                }
            }

            // Управление во время полёта
            if (GlobalUtils.Fly)
            {
                GameObject playercamera = Wrapper.GetPlayerCamera();
                VRCPlayer player = PlayerWrapper.GetCurrentPlayer();

                if (flyspeed <= 0f)
                    flyspeed = 1f;

                if (Input.GetKeyDown(KeyCode.LeftShift))
                    flyspeed *= 2f;
                if (Input.GetKeyUp(KeyCode.LeftShift))
                    flyspeed /= 2f;

                if (Input.mouseScrollDelta.y != 0)
                    flyspeed += Input.mouseScrollDelta.y;

                if (Input.GetKey(KeyCode.W))
                    player.transform.position += playercamera.transform.forward * flyspeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.S))
                    player.transform.position -= playercamera.transform.forward * flyspeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.A))
                    player.transform.position -= playercamera.transform.right * flyspeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.D))
                    player.transform.position += playercamera.transform.right * flyspeed * Time.deltaTime;

                if (Input.GetKey(KeyCode.E))
                    player.transform.position += playercamera.transform.up * flyspeed * Time.deltaTime;
                if (Input.GetKey(KeyCode.Q))
                    player.transform.position -= playercamera.transform.up * flyspeed * Time.deltaTime;
            }
        }
    }
}
