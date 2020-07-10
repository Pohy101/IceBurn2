using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace IceBurn.Utils
{
    class GlobalUtils
    {
        // нужные переменные сюда
        public static bool Fly = false;
        public static bool FreeCam = false;
        public static bool ESP = false;
        public static int flySpeed = 5;
        public static float brightness = 1f;
        public static float ownBrightness = 1f;
        public static int walkSpeed = 2;
        public static int cameraFOV = 65;
        public static Vector3 Gravity = Physics.gravity;

        // Доступ к клиенту
        public static bool CanUseClient = false;
        public static bool CanUseAll = false;

        // Anti Crash preset
        public static int max_polygons = 500000;
        public static int max_particles = 30000;
        public static int max_particle_sys = 20;
        public static int max_meshes = 10;

        public static void UpdatePlayerSpeed()
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null)
                return;
            LocomotionInputController componentInChildren = PlayerWrapper.GetCurrentPlayer().GetComponentInChildren<LocomotionInputController>();
            if (componentInChildren != null)
            {
                componentInChildren.runSpeed = walkSpeed;
                componentInChildren.walkSpeed = walkSpeed;
                componentInChildren.strafeSpeed = walkSpeed;
            }
        }

		public static string convert(WebResponse res)
		{
			string result = "";
			using (Stream responseStream = res.GetResponseStream())
			{
				using (StreamReader streamReader = new StreamReader(responseStream))
				{
					result = streamReader.ReadToEnd();
				}
			}
			res.Dispose();
			return result;
		}

		public static void ForceLogoutRPC(APIUser player)
        {
            try
            {
                GameObject gameObject = Resources.FindObjectsOfTypeAll<ModerationManager>()[0].gameObject;
                Networking.RPC(0, gameObject, "ForceLogoutRPC", new Il2CppSystem.Object[]
                {
                    (Il2CppSystem.Object)player.id
                });
            }
            catch
            {
            }
        }

        // телепорт в точку на экране
        public static void RayTeleport()
        {
            Ray ray = new Ray(Wrapper.GetPlayerCamera().transform.position, Wrapper.GetPlayerCamera().transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits.Length > 0)
            {
                RaycastHit raycastHit = hits[0];
                var thisPlayer = PlayerWrapper.GetCurrentPlayer();
                thisPlayer.transform.position = raycastHit.point;
            }
        }

        public static void ToggleColliders(bool Toggle)
        {
            Collider[] array = UnityEngine.Object.FindObjectsOfType<Collider>();
            Component component = PlayerWrapper.GetCurrentPlayer().GetComponents(Il2CppType.Of<Collider>()).FirstOrDefault<Component>();

            for (int i = 0; i < array.Length; i++)
            {
                Collider collider = array[i];
                bool flag = collider.GetComponent<PlayerSelector>() != null || collider.GetComponent<VRC_Pickup>() != null || collider.GetComponent<QuickMenu>() != null || collider.GetComponent<VRCStation>() != null || collider.GetComponent<VRC_AvatarPedestal>() != null; //ебать какая длинная строка

                if (!flag && collider != component)
                {
                    collider.enabled = Toggle;
                }
            }
        }
    }

	public class avi
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00004397 File Offset: 0x00002597
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0000439F File Offset: 0x0000259F
		[JsonProperty("name")]
		public string Name { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x000043A8 File Offset: 0x000025A8
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x000043B0 File Offset: 0x000025B0
		[JsonProperty("description")]
		public string Description { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x000043B9 File Offset: 0x000025B9
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x000043C1 File Offset: 0x000025C1
		[JsonProperty("authorId")]
		public string AuthorId { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000043CA File Offset: 0x000025CA
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x000043D2 File Offset: 0x000025D2
		[JsonProperty("authorName")]
		public string AuthorName { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000043DB File Offset: 0x000025DB
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x000043E3 File Offset: 0x000025E3
		[JsonProperty("imageUrl")]
		public string ImageUrl { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x000043EC File Offset: 0x000025EC
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x000043F4 File Offset: 0x000025F4
		[JsonProperty("thumbnailImageUrl")]
		public string ThumbnailImageUrl { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x000043FD File Offset: 0x000025FD
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x00004405 File Offset: 0x00002605
		[JsonProperty("assetUrl")]
		public string AssetUrl { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0000440E File Offset: 0x0000260E
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x00004416 File Offset: 0x00002616
		[JsonProperty("assetUrlObject")]
		public UrlObject AssetUrlObject { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0000441F File Offset: 0x0000261F
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x00004427 File Offset: 0x00002627
		[JsonProperty("tags")]
		public object[] Tags { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00004430 File Offset: 0x00002630
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x00004438 File Offset: 0x00002638
		[JsonProperty("releaseStatus")]
		public string ReleaseStatus { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00004441 File Offset: 0x00002641
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00004449 File Offset: 0x00002649
		[JsonProperty("version")]
		public long Version { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00004452 File Offset: 0x00002652
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x0000445A File Offset: 0x0000265A
		[JsonProperty("unityPackageUrl")]
		public string UnityPackageUrl { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00004463 File Offset: 0x00002663
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x0000446B File Offset: 0x0000266B
		[JsonProperty("unityPackageUrlObject")]
		public UrlObject UnityPackageUrlObject { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00004474 File Offset: 0x00002674
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x0000447C File Offset: 0x0000267C
		[JsonProperty("unityVersion")]
		public string UnityVersion { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00004485 File Offset: 0x00002685
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x0000448D File Offset: 0x0000268D
		[JsonProperty("assetVersion")]
		public long AssetVersion { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00004496 File Offset: 0x00002696
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x0000449E File Offset: 0x0000269E
		[JsonProperty("platform")]
		public string Platform { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x000044A7 File Offset: 0x000026A7
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x000044AF File Offset: 0x000026AF
		[JsonProperty("featured")]
		public bool Featured { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x000044B8 File Offset: 0x000026B8
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x000044C0 File Offset: 0x000026C0
		[JsonProperty("imported")]
		public bool Imported { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x000044C9 File Offset: 0x000026C9
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x000044D1 File Offset: 0x000026D1
		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x000044DA File Offset: 0x000026DA
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x000044E2 File Offset: 0x000026E2
		[JsonProperty("updated_at")]
		public DateTimeOffset UpdatedAt { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x000044EB File Offset: 0x000026EB
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x000044F3 File Offset: 0x000026F3
		[JsonProperty("id")]
		public string Id { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x000044FC File Offset: 0x000026FC
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x00004504 File Offset: 0x00002704
		[JsonProperty("unityPackages")]
		public UnityPackage[] UnityPackages { get; set; }
	}

	public class UnityPackage
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0000450D File Offset: 0x0000270D
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x00004515 File Offset: 0x00002715
		[JsonProperty("id")]
		public string Id { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0000451E File Offset: 0x0000271E
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x00004526 File Offset: 0x00002726
		[JsonProperty("assetUrl")]
		public string AssetUrl { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0000452F File Offset: 0x0000272F
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x00004537 File Offset: 0x00002737
		[JsonProperty("assetUrlObject")]
		public UrlObject AssetUrlObject { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00004540 File Offset: 0x00002740
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x00004548 File Offset: 0x00002748
		[JsonProperty("unityVersion")]
		public string UnityVersion { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00004551 File Offset: 0x00002751
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x00004559 File Offset: 0x00002759
		[JsonProperty("unitySortNumber")]
		public long UnitySortNumber { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x00004562 File Offset: 0x00002762
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0000456A File Offset: 0x0000276A
		[JsonProperty("assetVersion")]
		public long AssetVersion { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00004573 File Offset: 0x00002773
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x0000457B File Offset: 0x0000277B
		[JsonProperty("platform")]
		public string Platform { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00004584 File Offset: 0x00002784
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0000458C File Offset: 0x0000278C
		[JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
		public DateTimeOffset? CreatedAt { get; set; }
	}
}
