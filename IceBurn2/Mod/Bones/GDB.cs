using Harmony;
using Logger;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using VRC;
using VRC.Core;
using ConsoleColor = System.ConsoleColor;
using IntPtr = System.IntPtr;

namespace IceBurn.Mod.Bones
{
    public static class GDB
    {
        private static class NDBConfig
        {
            public static float distanceToDisable;
            public static float colliderSizeLimit;
            public static int dynamicBoneUpdateRate;
            public static bool distanceDisable;
            public static bool enabledByDefault;
            public static bool disallowInsideColliders;
            public static bool onlyForMyBones;
            public static bool onlyForMeAndFriends;
            public static bool disallowDesktoppers;
            public static bool enableBoundsCheck;
            public static float visiblityUpdateRate;
            public static bool onlyHandColliders;
            public static bool keybindsEnabled;
            public static bool onlyOptimize;
            public static int updateMode;
        }


        struct OriginalBoneInformation
        {
            public float updateRate;
            public float distanceToDisable;
            public List<DynamicBoneCollider> colliders;
            public DynamicBone referenceToOriginal;
            public bool distantDisable;
        }

        private static Dictionary<string, System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> avatarsInScene;
        private static Dictionary<string, List<OriginalBoneInformation>> originalSettings;
        public static Dictionary<string, System.Tuple<Renderer, DynamicBone[]>> avatarRenderers;
        private static GameObject localPlayer;
        //private Transform localPlayerReferenceTransform;
        //private Transform onlyFriendsButton; //OnlyFans haha
        private static bool enabled = true;

        private static float nextUpdateVisibility = 0;
        private const float visiblityUpdateRate = 1f;

        private static AvatarInstantiatedDelegate onAvatarInstantiatedDelegate;
        private static PlayerLeftDelegate onPlayerLeftDelegate;
        private static JoinedRoom onJoinedRoom;

        private static void Hook(IntPtr target, IntPtr detour)
        {
            Imports.Hook(target, detour);
        }

        private delegate void AvatarInstantiatedDelegate(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded);
        private delegate void PlayerLeftDelegate(IntPtr @this, IntPtr playerPtr);
        private delegate void JoinedRoom(IntPtr @this);


        private static void UiExpansionKit_AddSimpleMenuButton(Type uiKitApiType, int mode, string text, Action onClick, Action<GameObject> onShow)
        {
            uiKitApiType.GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { mode, text, onClick, onShow });
        }

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        static extern int MessageBox(IntPtr nWnd, string text, string title, uint type);

        public static unsafe void OnApplicationStartX()
        {
            RegisterModPrefs();
            enabled = NDBConfig.enabledByDefault;
            HookCallbackFunctions();
            OnModSettingsAppliedX();
        }

        private static unsafe void RegisterModPrefs()
        {
            ModPrefs.RegisterCategory("NDB", "Multiplayer Dynamic Bones");
            ModPrefs.RegisterPrefBool("NDB", "EnabledByDefault", true, "Enabled by default");
            ModPrefs.RegisterPrefBool("NDB", "OnlyMe", false, "Only I can interact with other bones");
            ModPrefs.RegisterPrefBool("NDB", "OnlyFriends", false, "Only me and friends can interact with my and friend's bones");
            ModPrefs.RegisterPrefBool("NDB", "DisallowDesktoppers", false, "Desktoppers's colliders and bones won't be multiplayer'd");
            ModPrefs.RegisterPrefBool("NDB", "DistanceDisable", true, "Disable bones if beyond a distance");
            ModPrefs.RegisterPrefFloat("NDB", "DistanceToDisable", 4f, "Distance limit");
            ModPrefs.RegisterPrefBool("NDB", "DisallowInsideColliders", true, "Disallow inside colliders");
            ModPrefs.RegisterPrefFloat("NDB", "ColliderSizeLimit", 1f, "Collider size limit");
            ModPrefs.RegisterPrefInt("NDB", "DynamicBoneUpdateRate", 60, "Dynamic bone update rate");
            ModPrefs.RegisterPrefBool("NDB", "EnableJustIfVisible", true, "Enable dynamic bones just if they are on view");
            ModPrefs.RegisterPrefFloat("NDB", "VisibilityUpdateRate", 1f, "Visibility update rate (seconds)");
            ModPrefs.RegisterPrefBool("NDB", "OnlyHandColliders", false, "Only enable colliders in hands");
            ModPrefs.RegisterPrefBool("NDB", "KeybindsEnabled", true, "Enable keyboard actuation(F1, F4 and F8)");
            ModPrefs.RegisterPrefBool("NDB", "OptimizeOnly", false, "Just optimize the dynamic bones of the scene, don't enable interaction");
            ModPrefs.RegisterPrefInt("NDB", "UpdateMode", 0, "A value of 2 will notify the user when a new version of the mod is avaiable, while 1 will not.");
        }

        private static unsafe void HookCallbackFunctions()
        {
            IntPtr funcToHook = (IntPtr)typeof(VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoUnique).GetField("NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_GameObject_VRC_AvatarDescriptor_Boolean_0", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);

            Hook(funcToHook, new System.Action<IntPtr, IntPtr, IntPtr, bool>(OnAvatarInstantiated).Method.MethodHandle.GetFunctionPointer());
            onAvatarInstantiatedDelegate = Marshal.GetDelegateForFunctionPointer<AvatarInstantiatedDelegate>(*(IntPtr*)funcToHook);
            IceLogger.Log(((onAvatarInstantiatedDelegate != null) ? "Hooked onJoinedRoom!" : "onAvatarInstantiatedDelegate: critical error!!"));

            funcToHook = (IntPtr)typeof(NetworkManager).GetField("NativeMethodInfoPtr_Method_Public_Void_Player_0", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);
            Hook(funcToHook, new System.Action<IntPtr, IntPtr>(OnPlayerLeft).Method.MethodHandle.GetFunctionPointer());
            onPlayerLeftDelegate = Marshal.GetDelegateForFunctionPointer<PlayerLeftDelegate>(*(IntPtr*)funcToHook);
            IceLogger.Log(((onPlayerLeftDelegate != null) ? "Hooked onJoinedRoom!" : "onPlayerLeftDelegate: critical error!!"));

            funcToHook = (IntPtr)typeof(NetworkManager).GetField("NativeMethodInfoPtr_OnJoinedRoom_Public_Virtual_Final_New_Void_3", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);
            Hook(funcToHook, new System.Action<IntPtr>(OnJoinedRoom).Method.MethodHandle.GetFunctionPointer());
            onJoinedRoom = Marshal.GetDelegateForFunctionPointer<JoinedRoom>(*(IntPtr*)funcToHook);
            IceLogger.Log(((onJoinedRoom != null) ? "Hooked onJoinedRoom!" : "onJoinedRoom: critical error!!"));

            if (onPlayerLeftDelegate == null || onAvatarInstantiatedDelegate == null || onJoinedRoom == null)
            {
                enabled = false;
                IceLogger.Log("Multiplayer Dynamic Bones mod suffered a critical error!");
            }
        }

        public static void OnModSettingsAppliedX()
        {
            NDBConfig.enabledByDefault = ModPrefs.GetBool("NDB", "EnabledByDefault");
            NDBConfig.disallowInsideColliders = ModPrefs.GetBool("NDB", "DisallowInsideColliders");
            NDBConfig.distanceToDisable = ModPrefs.GetFloat("NDB", "DistanceToDisable");
            NDBConfig.distanceDisable = ModPrefs.GetBool("NDB", "DistanceDisable");
            NDBConfig.colliderSizeLimit = ModPrefs.GetFloat("NDB", "ColliderSizeLimit");
            NDBConfig.onlyForMyBones = ModPrefs.GetBool("NDB", "OnlyMe");
            NDBConfig.onlyForMeAndFriends = ModPrefs.GetBool("NDB", "OnlyFriends");
            NDBConfig.dynamicBoneUpdateRate = ModPrefs.GetInt("NDB", "DynamicBoneUpdateRate");
            NDBConfig.disallowDesktoppers = ModPrefs.GetBool("NDB", "DisallowDesktoppers");
            NDBConfig.enableBoundsCheck = ModPrefs.GetBool("NDB", "EnableJustIfVisible");
            NDBConfig.visiblityUpdateRate = ModPrefs.GetFloat("NDB", "VisibilityUpdateRate");
            NDBConfig.onlyHandColliders = ModPrefs.GetBool("NDB", "OnlyHandColliders");
            NDBConfig.keybindsEnabled = ModPrefs.GetBool("NDB", "KeybindsEnabled");
            NDBConfig.onlyOptimize = ModPrefs.GetBool("NDB", "OptimizeOnly");
            NDBConfig.updateMode = ModPrefs.GetInt("NDB", "UpdateMode");

        }

        private static void OnJoinedRoom(IntPtr @this)
        {
            originalSettings = new Dictionary<string, List<OriginalBoneInformation>>();
            avatarsInScene = new Dictionary<string, System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>>();
            avatarRenderers = new Dictionary<string, System.Tuple<Renderer, DynamicBone[]>>();
            localPlayer = null;

            onJoinedRoom(@this);
            IceLogger.Log("New scene loaded. Reset...");
        }

        private static void OnPlayerLeft(IntPtr @this, IntPtr playerPtr)
        {
            Player player = new Player(playerPtr);

            if (!avatarsInScene.ContainsKey(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0) && !originalSettings.ContainsKey(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0))
            {
                onPlayerLeftDelegate(@this, playerPtr);
                return;

            }

            RemoveBonesOfGameObjectInAllPlayers(avatarsInScene[player.field_Internal_VRCPlayer_0.namePlate.prop_String_0].Item4);
            DeleteOriginalColliders(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0);
            RemovePlayerFromDict(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0);
            RemoveDynamicBonesFromVisibilityList(player.field_Internal_VRCPlayer_0.namePlate.prop_String_0);
            IceLogger.Log( $"Player {player.field_Internal_VRCPlayer_0.namePlate.prop_String_0} left the room. All his dynamic bones info was deleted");
            onPlayerLeftDelegate(@this, playerPtr);
        }

        private static void RecursiveHierarchyDump(Transform child, int c)
        {
            StringBuilder offs = new StringBuilder();
            for (int i = 0; i < c; i++) offs.Append('-');
            offs.Append(child.name);
            offs.Append("  Components: ");
            child.GetComponents<Component>().Do((b) => offs.Append(b.ToString() + " | "));
            MelonModLogger.Log(ConsoleColor.White, offs.ToString());
            for (int x = 0; x < child.childCount; x++)
            {
                RecursiveHierarchyDump(child.GetChild(x), c + 1);

            }
        }

        //private static bool hasDumpedIt = false;
        private static void OnAvatarInstantiated(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded)
        {
            onAvatarInstantiatedDelegate(@this, avatarPtr, avatarDescriptorPtr, loaded);

            try
            {
                if (loaded)
                {
                    GameObject avatar = new GameObject(avatarPtr);
                    //VRC.SDKBase.VRC_AvatarDescriptor avatarDescriptor = new VRC.SDKBase.VRC_AvatarDescriptor(avatarDescriptorPtr);


                    if (avatar.transform.root.gameObject.name.Contains("[Local]"))
                    {
                        localPlayer = avatar;
                    }

                    AddOrReplaceWithCleanup(
                        avatar.transform.root.GetComponentInChildren<VRCPlayer>().namePlate.prop_String_0,
                        new System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>(
                            avatar,
                            avatar.transform.root.GetComponentInChildren<VRCPlayer>().prop_VRCPlayerApi_0.IsUserInVR(),
                            avatar.GetComponentsInChildren<DynamicBone>(),
                            avatar.GetComponentsInChildren<DynamicBoneCollider>(),
                            APIUser.IsFriendsWith(avatar.transform.root.GetComponentInChildren<Player>().prop_APIUser_0.id)));

                    IceLogger.Log("New avatar loaded: ");
                    IceLogger.Log($"Added {avatar.transform.root.GetComponentInChildren<VRCPlayer>().namePlate.prop_String_0}");
                }
            }
            catch (Exception ex)
            {
                IceLogger.Error("An exception was thrown while working!\n" + ex.ToString());
            }
        }

        public static void AddOrReplaceWithCleanup(string key, System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> newValue)
        {
            foreach (DynamicBoneCollider col in newValue.Item4)
            {
                if (NDBConfig.disallowInsideColliders && col.m_Bound == DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                {
                    newValue.Item3.Do((b) => b.m_Colliders.Remove(col));
                    MelonModLogger.Log($"Removing bone {col.transform.name} because settings disallow inside colliders");
                    GameObject.Destroy(col);
                }
            }

            if (!avatarsInScene.ContainsKey(key))
            {
                SaveOriginalColliderList(key, newValue.Item3);
                AddToPlayerDict(key, newValue);
            }
            else
            {
                DeleteOriginalColliders(key);
                SaveOriginalColliderList(key, newValue.Item3);
                DynamicBoneCollider[] oldColliders = avatarsInScene[key].Item4;
                RemovePlayerFromDict(key);
                AddToPlayerDict(key, newValue);
                RemoveBonesOfGameObjectInAllPlayers(oldColliders);
                RemoveDynamicBonesFromVisibilityList(key);
                MelonModLogger.Log(ConsoleColor.Blue, $"User {key} swapped avatar, system updated");
            }
            if (enabled) AddBonesOfGameObjectToAllPlayers(newValue);
            if (newValue.Item1 != localPlayer) AddDynamicBonesToVisibilityList(key, newValue.Item3, newValue.Item1.GetComponentInChildren<SkinnedMeshRenderer>());
        }

        private static bool SelectBonesWithRules(KeyValuePair<string, System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> item)
        {
            bool valid = true;
            if (NDBConfig.onlyForMyBones) valid &= item.Value.Item1 == localPlayer;
            if (NDBConfig.onlyForMeAndFriends) valid &= item.Value.Item5 || (item.Value.Item1 == localPlayer);
            if (NDBConfig.disallowDesktoppers) valid &= item.Value.Item2 || (item.Value.Item1 == localPlayer);
            return valid;
        }

        private static bool SelectCollidersWithRules(KeyValuePair<string, System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> item)
        {
            bool valid = true;
            if (NDBConfig.onlyForMeAndFriends) valid &= item.Value.Item5 || (item.Value.Item1 == localPlayer);
            if (NDBConfig.disallowDesktoppers) valid &= item.Value.Item2;
            return valid;
        }

        private static bool ColliderMeetsRules(DynamicBoneCollider coll, System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> item)
        {
            bool valid = true;
            if (NDBConfig.onlyHandColliders) valid &= coll.transform.IsChildOf(item.Item1.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand).parent) || coll.transform.IsChildOf(item.Item1.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand).parent);
            return valid;
        }

        private static void ApplyBoneSettings(DynamicBone bone)
        {
            bone.m_DistantDisable = NDBConfig.distanceDisable;
            bone.m_DistanceToObject = NDBConfig.distanceToDisable;
            bone.m_UpdateRate = NDBConfig.dynamicBoneUpdateRate;
            bone.m_ReferenceObject = localPlayer?.transform ?? bone.m_ReferenceObject;
        }

        private static void AddAllCollidersToAllPlayers()
        {
            foreach (System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> player in avatarsInScene.Values)
            {
                AddBonesOfGameObjectToAllPlayers(player);
            }
        }

        private static void AddBonesOfGameObjectToAllPlayers(System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> player)
        {
            if (player.Item1 != localPlayer)
            {
                if (NDBConfig.onlyForMeAndFriends)
                {
                    if (!player.Item5)
                    {
                        MelonModLogger.Log(ConsoleColor.DarkYellow, $"Not adding bones of player {avatarsInScene.First((x) => x.Value.Item1 == player.Item1).Key} because settings only allow friends");
                        return;
                    }
                }
                if (NDBConfig.disallowDesktoppers)
                {
                    if (!player.Item2)
                    {
                        MelonModLogger.Log(ConsoleColor.DarkYellow, $"Not adding bones of player {avatarsInScene.First((x) => x.Value.Item1 == player.Item1).Key} because settings disallow desktopper");
                        return;
                    }
                }
            }

            foreach (DynamicBone db in player.Item3)
            {
                if (db == null) continue;
                ApplyBoneSettings(db);
            }

            if (NDBConfig.onlyOptimize) return;

            foreach (DynamicBone[] playersColliders in avatarsInScene.Where((x) => SelectBonesWithRules(x) && x.Value.Item1 != player.Item1).Select((x) => x.Value.Item3))
            {
                foreach (DynamicBone playerBone in playersColliders)
                {
                    foreach (DynamicBoneCollider playerCollider in player.Item4)
                    {
                        if (ColliderMeetsRules(playerCollider, player))
                        {
                            AddColliderToBone(playerBone, playerCollider);
                        }
                    }
                }
            }
        }

        private static void RemoveBonesOfGameObjectInAllPlayers(DynamicBoneCollider[] colliders)
        {
            foreach (DynamicBone[] dbs in avatarsInScene.Values.Select((x) => x.Item3))
            {
                foreach (DynamicBone db in dbs)
                {
                    foreach (DynamicBoneCollider dbc in colliders)
                    {
                        db.m_Colliders.Remove(dbc);
                    }
                }
            }
        }

        private static void AddColliderToDynamicBone(DynamicBone bone, DynamicBoneCollider dbc)
        {
#if DEBUG
            MelonModLogger.Log(ConsoleColor.Cyan, $"Adding {bone.m_Root.name} to {dbc.gameObject.name}");
#endif
            if (!bone.m_Colliders.Contains(dbc)) bone.m_Colliders.Add(dbc);
        }

        private static void AddColliderToBone(DynamicBone bone, DynamicBoneCollider collider)
        {
            if (NDBConfig.disallowInsideColliders && collider.m_Bound == DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
            {
                return;
            }

            if (collider.m_Radius > NDBConfig.colliderSizeLimit || collider.m_Height > NDBConfig.colliderSizeLimit)
            {
                return;
            }

            AddColliderToDynamicBone(bone, collider);
        }

        private static void AddDynamicBonesToVisibilityList(string player, DynamicBone[] dynamicBones, Renderer renderer)
        {
            avatarRenderers.Add(player, new System.Tuple<Renderer, DynamicBone[]>(renderer, dynamicBones));
        }

        private static void RemoveDynamicBonesFromVisibilityList(string player)
        {
            avatarRenderers.Remove(player);
        }



        public static void OnUpdateX()
        {
            if (avatarRenderers != null)
            {
                if (avatarRenderers.Count != 0 && NDBConfig.enableBoundsCheck) EnableIfVisible();
            }

            if (!NDBConfig.keybindsEnabled) return;

            if (Input.GetKeyDown(KeyCode.F11))
            {
                MelonModLogger.Log(ConsoleColor.DarkMagenta, "My bones have the following colliders attached:");
                avatarsInScene.Values.First((tup) => tup.Item1 == localPlayer).Item3.DoIf((bone) => bone != null, (bone) =>
                {
                    bone.m_Colliders.ToArray().Do((dbc) =>
                    {
                        try
                        {
                            MelonModLogger.Log(ConsoleColor.DarkMagenta, $"Bone {bone?.m_Root.name ?? "null"} has {dbc?.gameObject.name ?? "null"}");
                        }
                        catch (Exception) { };
                    });
                });

                MelonModLogger.Log(ConsoleColor.DarkMagenta, $"There are {avatarsInScene.Values.Aggregate(0, (acc, tup) => acc += tup.Item3.Length)} Dynamic Bones in scene");
                MelonModLogger.Log(ConsoleColor.DarkMagenta, $"There are {avatarsInScene.Values.Aggregate(0, (acc, tup) => acc += tup.Item4.Length)} Dynamic Bones Colliders in scene");

            }

        }

        private static void EnableIfVisible()
        {
            if (nextUpdateVisibility < Time.time)
            {
                foreach (System.Tuple<Renderer, DynamicBone[]> go in avatarRenderers.Values)
                {
                    if (go.Item1 == null) continue;
                    bool visible = go.Item1.isVisible;
                    foreach (DynamicBone db in go.Item2)
                    {
                        //if (db.enabled != visible) MelonModLogger.Log(ConsoleColor.DarkBlue, $"{db.gameObject.name} is now {((visible) ? "enabled" : "disabled")}");
                        db.enabled = visible;
                    }
                }
                nextUpdateVisibility = Time.time + NDBConfig.visiblityUpdateRate;
            }
        }

        public static void ToggleGDBState()
        {
            enabled = !enabled;
            MelonModLogger.Log(ConsoleColor.Green, $"NDBMod is now {((enabled == true) ? "enabled" : "disabled")}");
            if (!enabled)
                RestoreOriginalColliderList();
            else
                AddAllCollidersToAllPlayers();
        }

        private static void RemovePlayerFromDict(string name)
        {
            avatarsInScene.Remove(name);
        }

        private static void AddToPlayerDict(string name, System.Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool> value)
        {
            avatarsInScene.Add(name, value);
        }

        private static void DeleteOriginalColliders(string name)
        {
            originalSettings.Remove(name);
        }

        private static void SaveOriginalColliderList(string name, DynamicBone[] bones)
        {
            if (originalSettings.ContainsKey(name)) originalSettings.Remove(name);
            List<OriginalBoneInformation> ogInfo = new List<OriginalBoneInformation>(bones.Length);
            foreach (DynamicBone b in bones)
            {
                bones.Select((bone) =>
                {
                    return new OriginalBoneInformation() { distanceToDisable = bone.m_DistanceToObject, updateRate = bone.m_UpdateRate, distantDisable = bone.m_DistantDisable, colliders = new List<DynamicBoneCollider>(bone.m_Colliders.ToArrayExtension()), referenceToOriginal = bone };
                }).Do((info) => ogInfo.Add(info));
            }
            originalSettings.Add(name, ogInfo);
            MelonModLogger.Log(ConsoleColor.DarkGreen, $"Saved original dynamic bone info of player {name}");
        }

        private static void RestoreOriginalColliderList()
        {
            foreach (KeyValuePair<string, Tuple<GameObject, bool, DynamicBone[], DynamicBoneCollider[], bool>> player in avatarsInScene)
            {
                MelonModLogger.Log(ConsoleColor.DarkBlue, $"Restoring original settings for player {player.Key}");
                foreach (DynamicBone db in player.Value.Item3)
                {
                    if (originalSettings.TryGetValue(player.Key, out List<OriginalBoneInformation> origList))
                    {
                        try
                        {
                            origList.DoIf((x) => ReferenceEquals(x, db), (origData) =>
                            {
                                db.m_Colliders.Clear();
                                origData.colliders.ForEach((dbc) => db.m_Colliders.Add(dbc));
                                db.m_DistanceToObject = origData.distanceToDisable;
                                db.m_UpdateRate = origData.updateRate;
                                db.m_DistantDisable = origData.distantDisable;
                            });
                        }
                        catch (Exception e)
                        {
                            MelonModLogger.Log(ConsoleColor.Red, e.ToString());
                        }
                    }
                    else
                    {
                        MelonModLogger.Log(ConsoleColor.DarkYellow, $"Warning: could not find original dynamic bone info for {player.Key}'s bone {db.gameObject.name} . This means his bones won't be disabled!");
                    }
                }
            }
        }
    }

    public static class ListExtensions
    {
        public static T[] ToArrayExtension<T>(this Il2CppSystem.Collections.Generic.List<T> list)
        {
            T[] arr = new T[list.Count];
            for (int x = 0; x < list.Count; x++)
            {
                arr[x] = list[x];
            }
            return arr;
        }
    }
}