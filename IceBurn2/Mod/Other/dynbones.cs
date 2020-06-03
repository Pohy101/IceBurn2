using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;
/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Harmony;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;

namespace DBMod
{
	// Token: 0x02000002 RID: 2
	internal class NDB : MelonMod
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void Hook(IntPtr target, IntPtr detour)
		{
			typeof(Imports).GetMethod("Hook", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
			{
				target,
				detour
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
		private void AddToggleButton()
		{
			Transform transform = QuickMenu.prop_QuickMenu_0.transform;
			this.toggleButton = UnityEngine.Object.Instantiate<GameObject>(transform.Find("CameraMenu/BackButton").gameObject).transform;
			if (this.toggleButton == null)
			{
				MelonModLogger.Log(ConsoleColor.Red, "Couldn't add button for dynamic bones");
			}
			this.toggleButton.name = "NDBToggle";
			this.toggleButton.SetParent(transform.Find("ShortcutMenu"), false);
			this.toggleButton.GetComponentInChildren<Text>().text = "Press to " + (this.enabled ? "disable" : "enable") + " Dynamic Bones mod";
			float num = transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x - transform.Find("UserInteractMenu/BanButton").localPosition.x;
			float num2 = transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x - transform.Find("UserInteractMenu/BanButton").localPosition.x;
			this.toggleButton.localPosition = new Vector3(this.toggleButton.localPosition.x + num * (float)NDB.NDBConfig.toggleButtonX, this.toggleButton.localPosition.y + num2 * (float)NDB.NDBConfig.toggleButtonY, this.toggleButton.localPosition.z);
			this.toggleButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
			this.toggleButton.GetComponent<Button>().onClick.AddListener(new Action(() =>
			{
				try
				{
					this.ToggleState();
				}
				catch (Exception ex)
				{
					MelonModLogger.Log(ConsoleColor.Red, ex.ToString());
				}
			}));
			this.toggleButton.gameObject.SetActive(true);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000222C File Offset: 0x0000042C
		private void AddOnlyFriendsButton()
		{
			Transform transform = QuickMenu.prop_QuickMenu_0.transform;
			this.onlyFriendsButton = UnityEngine.Object.Instantiate<GameObject>(transform.Find("CameraMenu/BackButton").gameObject).transform;
			if (this.onlyFriendsButton == null)
			{
				MelonModLogger.Log(ConsoleColor.Red, "Couldn't add friends toggle button for dynamic bones");
			}
			this.onlyFriendsButton.name = "NDBFriends";
			this.onlyFriendsButton.SetParent(transform.Find("ShortcutMenu"), false);
			this.onlyFriendsButton.GetComponentInChildren<Text>().text = "Press to allow " + (NDB.NDBConfig.onlyForMeAndFriends ? "everyone" : "only friends") + " to have multiplayer dynamic bones.";
			float num = transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x - transform.Find("UserInteractMenu/BanButton").localPosition.x;
			float num2 = transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x - transform.Find("UserInteractMenu/BanButton").localPosition.x;
			this.onlyFriendsButton.localPosition = new Vector3(this.onlyFriendsButton.localPosition.x + num * 2f, this.onlyFriendsButton.localPosition.y + num2 * 1f, this.onlyFriendsButton.localPosition.z);
			this.onlyFriendsButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
			this.onlyFriendsButton.GetComponent<Button>().onClick.AddListener(new Action(() =>
			{
				try
				{
					NDB.NDBConfig.onlyForMeAndFriends = !NDB.NDBConfig.onlyForMeAndFriends;
					if (this.enabled)
					{
						this.ToggleState();
						this.ToggleState();
					}
					this.onlyFriendsButton.GetComponentInChildren<Text>().text = "Press to allow " + (NDB.NDBConfig.onlyForMeAndFriends ? "everyone" : "only friends") + " to have multiplayer dynamic bones.";
				}
				catch (Exception ex)
				{
					MelonModLogger.Log(ConsoleColor.Red, ex.ToString());
				}
			}));
			this.onlyFriendsButton.gameObject.SetActive(true);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023CB File Offset: 0x000005CB
		private void AddButtons()
		{
			this.AddToggleButton();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023D3 File Offset: 0x000005D3
		public override void VRChat_OnUiManagerInit()
		{
			if (NDB.NDBConfig.enableModUI)
			{
				this.AddButtons();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023E4 File Offset: 0x000005E4
		public override void OnApplicationStart()
		{
			NDB._Instance = this;
			ModPrefs.RegisterCategory("NDB", "Multiplayer Dynamic Bones");
			ModPrefs.RegisterPrefBool("NDB", "EnabledByDefault", true, "Enabled by default", false);
			ModPrefs.RegisterPrefBool("NDB", "OnlyMe", false, "Only I can interact with other bones", false);
			ModPrefs.RegisterPrefBool("NDB", "OnlyFriends", false, "Only me and friends can interact with my and friend's bones", false);
			ModPrefs.RegisterPrefBool("NDB", "DisallowDesktoppers", false, "Desktoppers's colliders and bones won't be multiplayer'd", false);
			ModPrefs.RegisterPrefBool("NDB", "DistanceDisable", true, "Disable bones if beyond a distance", false);
			ModPrefs.RegisterPrefFloat("NDB", "DistanceToDisable", 4f, "Distance limit", false);
			ModPrefs.RegisterPrefBool("NDB", "DisallowInsideColliders", true, "Disallow inside colliders", false);
			ModPrefs.RegisterPrefFloat("NDB", "ColliderSizeLimit", 1f, "Collider size limit", false);
			ModPrefs.RegisterPrefInt("NDB", "DynamicBoneUpdateRate", 60, "Dynamic bone update rate", false);
			ModPrefs.RegisterPrefBool("NDB", "EnableModUI", true, "Enables mod UI", false);
			ModPrefs.RegisterPrefInt("NDB", "ButtonPositionX", 1, "X position of button", false);
			ModPrefs.RegisterPrefInt("NDB", "ButtonPositionY", 1, "Y position of button", false);
			MelonModLogger.Log(ConsoleColor.DarkGreen, "Saved default configuration");
			NDB.NDBConfig.enabledByDefault = ModPrefs.GetBool("NDB", "EnabledByDefault");
			NDB.NDBConfig.disallowInsideColliders = ModPrefs.GetBool("NDB", "DisallowInsideColliders");
			NDB.NDBConfig.distanceToDisable = ModPrefs.GetFloat("NDB", "DistanceToDisable");
			NDB.NDBConfig.distanceDisable = ModPrefs.GetBool("NDB", "DistanceDisable");
			NDB.NDBConfig.colliderSizeLimit = ModPrefs.GetFloat("NDB", "ColliderSizeLimit");
			NDB.NDBConfig.onlyForMyBones = ModPrefs.GetBool("NDB", "OnlyMe");
			NDB.NDBConfig.onlyForMeAndFriends = ModPrefs.GetBool("NDB", "OnlyFriends");
			NDB.NDBConfig.dynamicBoneUpdateRate = ModPrefs.GetInt("NDB", "DynamicBoneUpdateRate");
			NDB.NDBConfig.disallowDesktoppers = ModPrefs.GetBool("NDB", "DisallowDesktoppers");
			NDB.NDBConfig.enableModUI = ModPrefs.GetBool("NDB", "EnableModUI");
			NDB.NDBConfig.toggleButtonX = ModPrefs.GetInt("NDB", "ButtonPositionX");
			NDB.NDBConfig.toggleButtonY = ModPrefs.GetInt("NDB", "ButtonPositionY");
			this.enabled = NDB.NDBConfig.enabledByDefault;
			IntPtr intPtr = (IntPtr)typeof(VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoObVoInBeInGaUnique).GetField("NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_GameObject_VRC_AvatarDescriptor_Boolean_0", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			NDB.Hook(intPtr, new Action<IntPtr, IntPtr, IntPtr, bool>(NDB.OnAvatarInstantiated).Method.MethodHandle.GetFunctionPointer());
			NDB.onAvatarInstantiatedDelegate = Marshal.GetDelegateForFunctionPointer<NDB.AvatarInstantiatedDelegate>(*(IntPtr*)((void*)intPtr));
			MelonModLogger.Log(ConsoleColor.Blue, "Hooked OnAvatarInstantiated? " + ((NDB.onAvatarInstantiatedDelegate != null) ? "Yes!" : "No: critical error!!"));
			IntPtr intPtr2 = (IntPtr)typeof(NetworkManager).GetField("NativeMethodInfoPtr_Method_Public_Void_Player_0", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			NDB.Hook(intPtr2, new Action<IntPtr, IntPtr>(NDB.OnPlayerLeft).Method.MethodHandle.GetFunctionPointer());
			NDB.onPlayerLeftDelegate = Marshal.GetDelegateForFunctionPointer<NDB.PlayerLeftDelegate>(*(IntPtr*)((void*)intPtr2));
			MelonModLogger.Log(ConsoleColor.Blue, "Hooked OnPlayerLeft? " + ((NDB.onPlayerLeftDelegate != null) ? "Yes!" : "No: critical error!!"));
			IntPtr intPtr3 = (IntPtr)typeof(NetworkManager).GetField("NativeMethodInfoPtr_OnJoinedRoom_Public_Virtual_Final_New_Void_2", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			NDB.Hook(intPtr3, new Action<IntPtr>(NDB.OnJoinedRoom).Method.MethodHandle.GetFunctionPointer());
			NDB.onJoinedRoom = Marshal.GetDelegateForFunctionPointer<NDB.JoinedRoom>(*(IntPtr*)((void*)intPtr3));
			MelonModLogger.Log(ConsoleColor.Blue, "Hooked OnJoinedRoom? " + ((NDB.onJoinedRoom != null) ? "Yes!" : "No: critical error!!"));
			MelonModLogger.Log(ConsoleColor.Green, "NDBMod is " + (this.enabled ? "enabled" : "disabled"));
			if (NDB.onPlayerLeftDelegate == null || NDB.onAvatarInstantiatedDelegate == null || NDB.onJoinedRoom == null)
			{
				this.enabled = false;
				MelonModLogger.Log(ConsoleColor.Red, "Multiplayer Dynamic Bones mod suffered a critical error! Please remove from the Mods folder to avoid game crashes! \nContact me for support.");
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000027D8 File Offset: 0x000009D8
		private static void OnJoinedRoom(IntPtr @this)
		{
			NDB._Instance.originalSettings = new Dictionary<string, List<NDB.OriginalBoneInformation>>();
			NDB._Instance.avatarsInScene = new Dictionary<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>>();
			NDB._Instance.localPlayer = null;
			NDB.onJoinedRoom(@this);
			MelonModLogger.Log(ConsoleColor.Blue, "New scene loaded; reset");
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002828 File Offset: 0x00000A28
		private static void OnPlayerLeft(IntPtr @this, IntPtr playerPtr)
		{
			Player player = new Player(playerPtr);
			if (!NDB._Instance.avatarsInScene.ContainsKey(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0) && !NDB._Instance.originalSettings.ContainsKey(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0))
			{
				NDB.onPlayerLeftDelegate(@this, playerPtr);
				return;
			}
			NDB._Instance.RemoveBonesOfGameObjectInAllPlayers(NDB._Instance.avatarsInScene[player.field_Internal_VRCPlayer_0.namePlate.prop_String_0].Item4);
			NDB._Instance.DeleteOriginalColliders(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0);
			NDB._Instance.RemovePlayerFromDict(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0);
			MelonModLogger.Log(ConsoleColor.Blue, "Player " + player.field_Internal_VRCPlayer_0.namePlate.prop_String_0 + " left the room so all his dynamic bones info was deleted");
			NDB.onPlayerLeftDelegate(@this, playerPtr);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002920 File Offset: 0x00000B20
		private static void RecursiveHierarchyDump(Transform child, int c)
		{
			StringBuilder offs = new StringBuilder();
			for (int i = 0; i < c; i++)
			{
				offs.Append('-');
			}
			offs.Append(child.name);
			offs.Append("  Components: ");
			CollectionExtensions.Do<Component>(child.GetComponents<Component>(), delegate (Component b)
			{
				offs.Append(b.GetType().Name + " | ");
			});
			MelonModLogger.Log(ConsoleColor.White, offs.ToString());
			for (int j = 0; j < child.childCount; j++)
			{
				NDB.RecursiveHierarchyDump(child.GetChild(j), c + 1);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000029C4 File Offset: 0x00000BC4
		private static void OnAvatarInstantiated(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded)
		{
			NDB.onAvatarInstantiatedDelegate(@this, avatarPtr, avatarDescriptorPtr, loaded);
			try
			{
				if (loaded)
				{
					GameObject gameObject = new GameObject(avatarPtr);
					if (gameObject.transform.root.gameObject.name.Contains("[Local]"))
					{
						NDB._Instance.localPlayer = gameObject;
					}
					NDB._Instance.AddOrReplaceWithCleanup(gameObject.transform.root.GetComponentInChildren<VRCPlayer>().namePlate.prop_String_0, new Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>(gameObject, gameObject.transform.root.GetComponentInChildren<VRCPlayer>().prop_VRCPlayerApi_0.IsUserInVR(), gameObject.GetComponentsInChildren<DynamicBone>(), gameObject.GetComponentsInChildren<DynamicBoneCollider>(), APIUser.IsFriendsWith(gameObject.transform.root.GetComponentInChildren<Player>().Method_Internal_get_APIUser_0().id)));
					MelonModLogger.Log(ConsoleColor.Blue, "New avatar loaded, added to avatar list");
					MelonModLogger.Log(ConsoleColor.Green, "Added " + gameObject.transform.root.GetComponentInChildren<VRCPlayer>().namePlate.prop_String_0);
				}
			}
			catch (Exception ex)
			{
				MelonModLogger.LogError("An exception was thrown while working!\n" + ex.ToString());
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public void AddOrReplaceWithCleanup(string key, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> newValue)
		{
			DynamicBoneCollider[] item = newValue.Item4;
			for (int i = 0; i < item.Length; i++)
			{
				DynamicBoneCollider col = item[i];
				if (NDB.NDBConfig.disallowInsideColliders && col.m_Bound == DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
				{
					CollectionExtensions.Do<DynamicBone>(newValue.Item3, delegate (DynamicBone b)
					{
						b.m_Colliders.Remove(col);
					});
					MelonModLogger.Log(ConsoleColor.Yellow, "Removing bone " + col.transform.name + " because settings disallow inside colliders");
					UnityEngine.Object.Destroy(col);
				}
			}
			if (!this.avatarsInScene.ContainsKey(key))
			{
				this.SaveOriginalColliderList(key, newValue.Item3);
				this.AddToPlayerDict(key, newValue);
			}
			else
			{
				this.DeleteOriginalColliders(key);
				this.SaveOriginalColliderList(key, newValue.Item3);
				DynamicBoneCollider[] item2 = this.avatarsInScene[key].Item4;
				this.RemovePlayerFromDict(key);
				this.AddToPlayerDict(key, newValue);
				this.RemoveBonesOfGameObjectInAllPlayers(item2);
				MelonModLogger.Log(ConsoleColor.Blue, "User " + key + " swapped avatar, system updated");
			}
			if (this.enabled)
			{
				this.AddBonesOfGameObjectToAllPlayers(newValue);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002C0C File Offset: 0x00000E0C
		private bool SelectBonesWithRules(KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> item)
		{
			bool flag = true;
			if (NDB.NDBConfig.onlyForMeAndFriends)
			{
				flag &= (item.Value.Item5 || item.Value.Item1 == this.localPlayer);
			}
			if (NDB.NDBConfig.disallowDesktoppers)
			{
				flag &= (item.Value.Item2 || item.Value.Item1 == this.localPlayer);
			}
			return flag;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002C80 File Offset: 0x00000E80
		private bool SelectCollidersWithRules(KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> item)
		{
			bool flag = true;
			if (NDB.NDBConfig.onlyForMeAndFriends)
			{
				flag &= (item.Value.Item5 || item.Value.Item1 == this.localPlayer);
			}
			if (NDB.NDBConfig.disallowDesktoppers)
			{
				flag &= item.Value.Item2;
			}
			return flag;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002CD8 File Offset: 0x00000ED8
		private void ApplyBoneSettings(DynamicBone bone)
		{
			bone.m_DistantDisable = NDB.NDBConfig.distanceDisable;
			bone.m_DistanceToObject = NDB.NDBConfig.distanceToDisable;
			bone.m_UpdateRate = (float)NDB.NDBConfig.dynamicBoneUpdateRate;
			GameObject gameObject = this.localPlayer;
			bone.m_ReferenceObject = (((gameObject != null) ? gameObject.transform : null) ?? bone.m_ReferenceObject);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002D2C File Offset: 0x00000F2C
		private void AddAllCollidersToAllPlayers()
		{
			foreach (Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> player in this.avatarsInScene.Values)
			{
				this.AddBonesOfGameObjectToAllPlayers(player);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002D84 File Offset: 0x00000F84
		private void AddBonesOfGameObjectToAllPlayers(Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> player)
		{
			if (player.Item1 != this.localPlayer)
			{
				if (NDB.NDBConfig.onlyForMeAndFriends && !player.Item5)
				{
					MelonModLogger.Log(ConsoleColor.DarkYellow, "Not adding bones of player " + this.avatarsInScene.First((KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> x) => x.Value.Item1 == player.Item1).Key + " because settings only allow friends");
					return;
				}
				if (NDB.NDBConfig.disallowDesktoppers)
				{
					MelonModLogger.Log(ConsoleColor.DarkYellow, "Not adding bones of player " + this.avatarsInScene.First((KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> x) => x.Value.Item1 == player.Item1).Key + " because settings disallow desktopper");
					if (!player.Item2)
					{
						return;
					}
				}
			}
			foreach (DynamicBone dynamicBone in player.Item3)
			{
				if (!(dynamicBone == null))
				{
					this.ApplyBoneSettings(dynamicBone);
				}
			}
			IEnumerable<KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>>> source = this.avatarsInScene;
			Func<KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>>, bool> <> 9__2;
			Func<KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>>, bool> predicate;
			if ((predicate = <> 9__2) == null)
			{
				predicate = (<> 9__2 = ((KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> x) => this.SelectBonesWithRules(x) && x.Value.Item1 != player.Item1));
			}
			foreach (DynamicBone[] array in from x in source.Where(predicate)
											select x.Value.Item3)
			{
				foreach (DynamicBone bone in array)
				{
					foreach (DynamicBoneCollider collider in player.Item4)
					{
						this.AddColliderToBone(bone, collider);
					}
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002F50 File Offset: 0x00001150
		private void RemoveBonesOfGameObjectInAllPlayers(DynamicBoneCollider[] colliders)
		{
			foreach (DynamicBone[] array in from x in this.avatarsInScene.Values
											select x.Item3)
			{
				foreach (DynamicBone dynamicBone in array)
				{
					foreach (DynamicBoneCollider dynamicBoneCollider in colliders)
					{
						dynamicBone.m_Colliders.Remove(dynamicBoneCollider);
					}
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002FFC File Offset: 0x000011FC
		private void AddColliderToDynamicBone(DynamicBone bone, DynamicBoneCollider dbc)
		{
			if (!bone.m_Colliders.Contains(dbc))
			{
				bone.m_Colliders.Add(dbc);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003018 File Offset: 0x00001218
		private void AddColliderToBone(DynamicBone bone, DynamicBoneCollider collider)
		{
			if (NDB.NDBConfig.disallowInsideColliders && collider.m_Bound == DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
			{
				return;
			}
			if (collider.m_Radius > NDB.NDBConfig.colliderSizeLimit || collider.m_Height > NDB.NDBConfig.colliderSizeLimit)
			{
				return;
			}
			this.AddColliderToDynamicBone(bone, collider);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003050 File Offset: 0x00001250
		public override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.F8))
			{
				MelonModLogger.Log(ConsoleColor.DarkMagenta, "My bones have the following colliders attached:");
				CollectionExtensions.DoIf<DynamicBone>(this.avatarsInScene.Values.First((Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> tup) => tup.Item1 == this.localPlayer).Item3, (DynamicBone bone) => bone != null, delegate (DynamicBone bone)
				{
					CollectionExtensions.Do<DynamicBoneCollider>(bone.m_Colliders.ToArray(), delegate (DynamicBoneCollider dbc)
					{
						try
						{
							ConsoleColor consoleColor = ConsoleColor.DarkMagenta;
							string str = "Bone ";
							MelonModLogger.Log(consoleColor, str + (((bone != null) ? bone.m_Root.name : null) ?? "null") + " has " + (((dbc != null) ? dbc.gameObject.name : null) ?? "null"));
						}
						catch (Exception)
						{
						}
					});
				});
				MelonModLogger.Log(ConsoleColor.DarkMagenta, string.Format("There are {0} Dynamic Bones in scene", this.avatarsInScene.Values.Aggregate(0, (int acc, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> tup) => acc += tup.Item3.Length)));
				MelonModLogger.Log(ConsoleColor.DarkMagenta, string.Format("There are {0} Dynamic Bones Colliders in scene", this.avatarsInScene.Values.Aggregate(0, (int acc, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> tup) => acc += tup.Item4.Length)));
			}
			if (Input.GetKeyDown(KeyCode.F1))
			{
				this.ToggleState();
			}
			if (Input.GetKeyDown(KeyCode.F4))
			{
				MelonModLogger.Log(ConsoleColor.Red, "List of avatar in dict:");
				foreach (string text in this.avatarsInScene.Keys)
				{
					MelonModLogger.Log(ConsoleColor.DarkGreen, text);
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000031DC File Offset: 0x000013DC
		private void ToggleState()
		{
			this.enabled = !this.enabled;
			MelonModLogger.Log(ConsoleColor.Green, "NDBMod is now " + (this.enabled ? "enabled" : "disabled"));
			if (!this.enabled)
			{
				this.RestoreOriginalColliderList();
			}
			else
			{
				this.AddAllCollidersToAllPlayers();
			}
			this.toggleButton.GetComponentInChildren<Text>().text = "Press to " + (this.enabled ? "disable" : "enable") + " Dynamic Bones mod";
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003266 File Offset: 0x00001466
		private void RemovePlayerFromDict(string name)
		{
			this.avatarsInScene.Remove(name);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003275 File Offset: 0x00001475
		private void AddToPlayerDict(string name, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> value)
		{
			this.avatarsInScene.Add(name, value);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003284 File Offset: 0x00001484
		private void DeleteOriginalColliders(string name)
		{
			this.originalSettings.Remove(name);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003294 File Offset: 0x00001494
		private void SaveOriginalColliderList(string name, DynamicBone[] bones)
		{
			if (this.originalSettings.ContainsKey(name))
			{
				this.originalSettings.Remove(name);
			}
			List<NDB.OriginalBoneInformation> ogInfo = new List<NDB.OriginalBoneInformation>(bones.Length);
			Action<NDB.OriginalBoneInformation> <> 9__1;
			foreach (DynamicBone dynamicBone in bones)
			{
				IEnumerable<NDB.OriginalBoneInformation> enumerable = from bone in bones
																	  select new NDB.OriginalBoneInformation
																	  {
																		  distanceToDisable = bone.m_DistanceToObject,
																		  updateRate = bone.m_UpdateRate,
																		  distantDisable = bone.m_DistantDisable,
																		  colliders = new List<DynamicBoneCollider>(bone.m_Colliders.ToArrayExtension<DynamicBoneCollider>()),
																		  referenceToOriginal = bone
																	  };
				Action<NDB.OriginalBoneInformation> action;
				if ((action = <> 9__1) == null)
				{
					action = (<> 9__1 = delegate (NDB.OriginalBoneInformation info)
					{
						ogInfo.Add(info);
					});
				}
				CollectionExtensions.Do<NDB.OriginalBoneInformation>(enumerable, action);
			}
			this.originalSettings.Add(name, ogInfo);
			MelonModLogger.Log(ConsoleColor.DarkGreen, "Saved original dynamic bone info of player " + name);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003350 File Offset: 0x00001550
		private void RestoreOriginalColliderList()
		{
			foreach (KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> keyValuePair in this.avatarsInScene)
			{
				MelonModLogger.Log(ConsoleColor.DarkBlue, "Restoring original settings for player " + keyValuePair.Key);
				DynamicBone[] item = keyValuePair.Value.Item3;
				for (int i = 0; i < item.Length; i++)
				{
					DynamicBone db = item[i];
					List<NDB.OriginalBoneInformation> list;
					if (this.originalSettings.TryGetValue(keyValuePair.Key, out list))
					{
						Action<DynamicBoneCollider> <> 9__2;
						CollectionExtensions.DoIf<NDB.OriginalBoneInformation>(list, (NDB.OriginalBoneInformation x) => x.referenceToOriginal == db, delegate (NDB.OriginalBoneInformation origData)
						{
							db.m_Colliders.Clear();
							List<DynamicBoneCollider> colliders = origData.colliders;
							Action<DynamicBoneCollider> action;
							if ((action = <> 9__2) == null)
							{
								action = (<> 9__2 = delegate (DynamicBoneCollider dbc)
								{
									db.m_Colliders.Add(dbc);
								});
							}
							colliders.ForEach(action);
							db.m_DistanceToObject = origData.distanceToDisable;
							db.m_UpdateRate = origData.updateRate;
							db.m_DistantDisable = origData.distantDisable;
						});
					}
					else
					{
						MelonModLogger.Log(ConsoleColor.DarkYellow, string.Concat(new string[]
						{
							"Warning: could not find original dynamic bone info for ",
							keyValuePair.Key,
							"'s bone ",
							db.gameObject.name,
							" . This means his bones won't be disabled!"
						}));
					}
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private static NDB _Instance;

		// Token: 0x04000002 RID: 2
		private Dictionary<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> avatarsInScene;

		// Token: 0x04000003 RID: 3
		private Dictionary<string, List<NDB.OriginalBoneInformation>> originalSettings;

		// Token: 0x04000004 RID: 4
		private GameObject localPlayer;

		// Token: 0x04000005 RID: 5
		private Transform toggleButton;

		// Token: 0x04000006 RID: 6
		private Transform onlyFriendsButton;

		// Token: 0x04000007 RID: 7
		private bool enabled = true;

		// Token: 0x04000008 RID: 8
		private static NDB.AvatarInstantiatedDelegate onAvatarInstantiatedDelegate;

		// Token: 0x04000009 RID: 9
		private static NDB.PlayerLeftDelegate onPlayerLeftDelegate;

		// Token: 0x0400000A RID: 10
		private static NDB.JoinedRoom onJoinedRoom;

		// Token: 0x02000004 RID: 4
		private static class NDBConfig
		{
			// Token: 0x0400000B RID: 11
			public static float distanceToDisable;

			// Token: 0x0400000C RID: 12
			public static float colliderSizeLimit;

			// Token: 0x0400000D RID: 13
			public static int dynamicBoneUpdateRate;

			// Token: 0x0400000E RID: 14
			public static bool distanceDisable;

			// Token: 0x0400000F RID: 15
			public static bool enabledByDefault;

			// Token: 0x04000010 RID: 16
			public static bool disallowInsideColliders;

			// Token: 0x04000011 RID: 17
			public static bool onlyForMyBones;

			// Token: 0x04000012 RID: 18
			public static bool onlyForMeAndFriends;

			// Token: 0x04000013 RID: 19
			public static bool disallowDesktoppers;

			// Token: 0x04000014 RID: 20
			public static bool enableModUI;

			// Token: 0x04000015 RID: 21
			public static int toggleButtonX;

			// Token: 0x04000016 RID: 22
			public static int toggleButtonY;
		}

		// Token: 0x02000005 RID: 5
		private struct OriginalBoneInformation
		{
			// Token: 0x04000017 RID: 23
			public float updateRate;

			// Token: 0x04000018 RID: 24
			public float distanceToDisable;

			// Token: 0x04000019 RID: 25
			public List<DynamicBoneCollider> colliders;

			// Token: 0x0400001A RID: 26
			public DynamicBone referenceToOriginal;

			// Token: 0x0400001B RID: 27
			public bool distantDisable;
		}

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000021 RID: 33
		private delegate void AvatarInstantiatedDelegate(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded);

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x06000025 RID: 37
		private delegate void PlayerLeftDelegate(IntPtr @this, IntPtr playerPtr);

		// Token: 0x02000008 RID: 8
		// (Invoke) Token: 0x06000029 RID: 41
		private delegate void JoinedRoom(IntPtr @this);
	}
}
*/

namespace IceBurn.Mod.Other
{
	// Token: 0x0200001D RID: 29
	public class dynbones
	{
		// Token: 0x060000DF RID: 223 RVA: 0x000093B8 File Offset: 0x000075B8
		public static void tracker(string avid, GameObject aviobj, string name = "")
		{
			Il2CppArrayBase<DynamicBone> componentsInChildren = aviobj.GetComponentsInChildren<DynamicBone>(true);
			Il2CppArrayBase<DynamicBoneCollider> componentsInChildren2 = aviobj.GetComponentsInChildren<DynamicBoneCollider>(true);
			List<dynbones.bones> list = new List<dynbones.bones>();
			IceLogger.Log(string.Concat(new string[]
			{
				"adding => bones ",
				componentsInChildren.Count.ToString(),
				" | colliders ",
				componentsInChildren2.Count.ToString(),
				" | user ",
				name
			}));
			for (int i = 0; i < componentsInChildren.Count; i++)
			{
				list.Add(new dynbones.bones
				{
					bone = componentsInChildren[i]
				});
			}
			dynbones.avatars value = new dynbones.avatars
			{
				extra_collisions = list
			};
			dynbones.map.Add(avid, value);
			dynbones.update(avid, componentsInChildren2);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000947C File Offset: 0x0000767C
		public static void update(string avid, Il2CppArrayBase<DynamicBoneCollider> cols)
		{
			foreach (KeyValuePair<string, dynbones.avatars> keyValuePair in dynbones.map)
			{
				if (keyValuePair.Key != avid)
				{
					foreach (dynbones.bones bones in keyValuePair.Value.extra_collisions)
					{
						for (int i = 0; i < cols.Count; i++)
						{
							DynamicBoneCollider dynamicBoneCollider = cols[i];
							bones.bone.m_Colliders.Add(dynamicBoneCollider);
							bones.Colliders.Add(dynamicBoneCollider);
						}
					}
				}
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000955C File Offset: 0x0000775C
		public static void remove(string avid)
		{
			if (!dynbones.map.ContainsKey(avid))
			{
				return;
			}
			dynbones.map[avid].extra_collisions.ForEach(delegate (dynbones.bones bone)
			{
				bone.Colliders.ForEach(delegate (DynamicBoneCollider col)
				{
					bone.bone.m_Colliders.Remove(col);
				});
			});
			dynbones.map.Remove(avid);
		}

		// Token: 0x0400009F RID: 159
		public static Dictionary<string, dynbones.avatars> map = new Dictionary<string, dynbones.avatars>();

		// Token: 0x0200003C RID: 60
		public class bones
		{
			// Token: 0x1700001F RID: 31
			// (get) Token: 0x06000178 RID: 376 RVA: 0x0000B50D File Offset: 0x0000970D
			// (set) Token: 0x06000179 RID: 377 RVA: 0x0000B515 File Offset: 0x00009715
			public DynamicBone bone { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x0600017A RID: 378 RVA: 0x0000B51E File Offset: 0x0000971E
			// (set) Token: 0x0600017B RID: 379 RVA: 0x0000B526 File Offset: 0x00009726
			public List<DynamicBoneCollider> Colliders { get; set; } = new List<DynamicBoneCollider>();
		}

		// Token: 0x0200003D RID: 61
		public class avatars
		{
			// Token: 0x17000021 RID: 33
			// (get) Token: 0x0600017D RID: 381 RVA: 0x0000B542 File Offset: 0x00009742
			// (set) Token: 0x0600017E RID: 382 RVA: 0x0000B54A File Offset: 0x0000974A
			public List<dynbones.bones> extra_collisions { get; set; } = new List<dynbones.bones>();
		}
	}
}
