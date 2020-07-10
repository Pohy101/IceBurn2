using IceBurn.Utils;
using Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace IceBurn.API
{
    public class AvatarListAPI
    {
		AvatarListAPI avatarListAPI = new AvatarListAPI();
		public GameObject ui_object;
		public static UiAvatarList ui_avatar_list;
		public UiAvatarList listing_avatars;
		public Button listing_button;
		public Text listing_text;

		public static UiAvatarList get_avatar_list()
		{
			if (AvatarListAPI.ui_avatar_list == null)
			{
				GameObject gameObject = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar").transform.Find("Vertical Scroll View/Viewport/Content").transform.Find("Favorite Avatar List").gameObject;
				gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
				gameObject.transform.Find("Button").GetComponentInChildren<Text>().text = "New List";
				UiAvatarList component = gameObject.GetComponent<UiAvatarList>();
				component.category = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLi9vUnique.SpecificList;
				component.StopAllCoroutines();
				gameObject.SetActive(false);
				AvatarListAPI.ui_avatar_list = component;
			}
			return AvatarListAPI.ui_avatar_list;
		}

		public AvatarListAPI setup(int i, string listname)
		{
			UiAvatarList avatarList = AvatarListAPI.get_avatar_list();
			avatarListAPI.ui_object = UnityEngine.Object.Instantiate<GameObject>(avatarList.gameObject, avatarList.transform.parent);
			avatarListAPI.ui_object.transform.SetSiblingIndex(i);
			avatarListAPI.listing_avatars = avatarListAPI.ui_object.gameObject.GetComponent<UiAvatarList>();
			avatarListAPI.listing_button = avatarListAPI.listing_avatars.GetComponentInChildren<Button>();
			avatarListAPI.listing_text = avatarListAPI.listing_avatars.GetComponentInChildren<Text>();
			avatarListAPI.listing_text.text = listname;
			avatarListAPI.listing_avatars.hideWhenEmpty = true;
			avatarListAPI.listing_avatars.clearUnseenListOnCollapse = true;
			avatarListAPI.ui_object.SetActive(true);
			return avatarListAPI;
		}

		public void set_action(Action act)
		{
			listing_button.onClick = new Button.ButtonClickedEvent();
			listing_button.onClick.AddListener(act);
		}
	}
	public class AvatarStruct
	{
		public string ThumbnailImageUrl;
		public string AvatarID;
		public string Name;
	}

	public class AvatarConfig
	{
		public static AvatarConfig config;
		public static void Save()
		{
			if (AvatarConfig.config == null)
				return;

			AvatarConfig.AvatarList.Reverse();
			File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\AvatarFav\\Avatars.json"), JsonConvert.SerializeObject(AvatarConfig.AvatarList, Formatting.Indented));
			AvatarConfig.AvatarList.Reverse();
		}

		public static void Load()
		{
			if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\AvatarFav\\Avatars.json")))
			{
				AvatarConfig.config = new AvatarConfig();
				AvatarConfig.Save();
				return;
			}

			if (AvatarConfig.config == null)
				AvatarConfig.config = new AvatarConfig();

			AvatarConfig.AvatarList.Clear();
			AvatarConfig.AvatarList = JsonConvert.DeserializeObject<List<AvatarStruct>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData\\IceBurn2\\AvatarFav\\Avatars.json")));
		}

		public static List<AvatarStruct> AvatarList = new List<AvatarStruct>
		{
			new AvatarStruct
			{
				ThumbnailImageUrl = "https://api.vrchat.cloud/api/1/image/file_904fa567-bb25-4cb8-9cd6-0f1ccd0261a8/3/256",
				AvatarID = "avtr_71bd9db5-a7be-4427-a584-517b6203eb6f",
				Name = "Loli Kon Quest"
			}
		};
	}

	public class AvatarUtils
	{
		// Token: 0x0600077C RID: 1916 RVA: 0x00022C5C File Offset: 0x00020E5C
		public static List<avi> get_public_avatars(string userid)
		{
			if (userid == "" || userid == null)
			{
				return null;
			}
			WebRequest webRequest = WebRequest.Create("https://api.vrchat.cloud/api/1/avatars?apiKey=JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26&userId=" + userid);
			ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate c, X509Chain cc, SslPolicyErrors ssl) => true);
			return JsonConvert.DeserializeObject<List<avi>>(GlobalUtils.convert(webRequest.GetResponse()));
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00022CC0 File Offset: 0x00020EC0
		public static void update_list(IEnumerable<string> arr, UiAvatarList avilist)
		{
			avilist.field_Private_Dictionary_2_String_ApiAvatar_0.Clear();
			foreach (string text in arr)
				if (!avilist.field_Private_Dictionary_2_String_ApiAvatar_0.ContainsKey(text))
					avilist.field_Private_Dictionary_2_String_ApiAvatar_0.Add(text, null);

			avilist.specificListIds = arr.ToArray<string>();
			avilist.Method_Protected_Abstract_Virtual_New_Void_Int32_0(0);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00022D40 File Offset: 0x00020F40
		public static void setup(List<AvatarStruct> avatars, UiAvatarList avilist)
		{
			avilist.field_Private_Dictionary_2_String_ApiAvatar_0.Clear();
			for (int i = 0; i < avatars.Count<AvatarStruct>(); i++)
			{
				AvatarStruct avatar_struct = avatars[i];
				ApiAvatar apiAvatar = new ApiAvatar
				{
					id = avatar_struct.AvatarID,
					thumbnailImageUrl = avatar_struct.ThumbnailImageUrl,
					name = avatar_struct.Name
				};

				if (!avilist.field_Private_Dictionary_2_String_ApiAvatar_0.ContainsKey(avatar_struct.AvatarID))
					avilist.field_Private_Dictionary_2_String_ApiAvatar_0.Add(avatar_struct.AvatarID, apiAvatar);
			}
			avilist.specificListIds = (from x in avatars
									   select x.AvatarID).ToArray<string>();
			avilist.Method_Protected_Abstract_Virtual_New_Void_Int32_0(0);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00022DFC File Offset: 0x00020FFC
		public static void add_to_list(ApiAvatar api)
		{
			if (api == null)
				return;

			if (api.releaseStatus == "private" && api.authorId != APIUser.CurrentUser.id)
				return;

			if (!AvatarConfig.AvatarList.Any((AvatarStruct x) => x.AvatarID == api.id))
			{
				AvatarConfig.AvatarList.Reverse();
				AvatarConfig.AvatarList.Add(new AvatarStruct
				{
					AvatarID = api.id,
					Name = api.name,
					ThumbnailImageUrl = api.thumbnailImageUrl
				});
				AvatarConfig.AvatarList.Reverse();
			}

			else
				AvatarConfig.AvatarList.RemoveAll((AvatarStruct x) => x.AvatarID == api.id);

			AvatarConfig.Save();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00022EE0 File Offset: 0x000210E0
		public static void update(List<string> arr, UiAvatarList avilist)
		{
			avilist.field_Private_Dictionary_2_String_ApiAvatar_0.Clear();
			foreach (string text in arr)
				avilist.field_Private_Dictionary_2_String_ApiAvatar_0.Add(text, null);

			avilist.specificListIds = arr.ToArray();
			avilist.Method_Protected_Abstract_Virtual_New_Void_Int32_0(0);
		}
	}
}
