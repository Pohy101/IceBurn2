using System;
using System.Collections.Generic;
using IceBurn.Utils;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRCSDK2;

namespace IceBurn.Mod.Other
{
    // Token: 0x0200001A RID: 26
    public static class anticrash
    {
        // Token: 0x060000CE RID: 206 RVA: 0x00008BB4 File Offset: 0x00006DB4
        public static void set_pickups(bool state)
        {
            foreach (VRC_Pickup vrc_Pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
            {
                if (!(vrc_Pickup == null) && !vrc_Pickup.name.Contains("ViewFinder"))
                {
                    vrc_Pickup.gameObject.SetActive(state);
                }
            }
        }

        // Token: 0x060000CF RID: 207 RVA: 0x00008C20 File Offset: 0x00006E20
        public static bool particle_check(Player user)
        {
            Il2CppArrayBase<ParticleSystem> componentsInChildren = user.GetComponentsInChildren<ParticleSystem>(true);
            if (componentsInChildren == null || componentsInChildren.Count == 0)
            {
                return false;
            }
            int num = Wrapper.get_particles_max(user);
            int num2 = Wrapper.get_particle_mesh_polys(user);
            int num3 = Wrapper.get_particle_systems(user);
            if (num >= GlobalUtils.max_particles || num2 >= GlobalUtils.max_particles || num3 >= GlobalUtils.max_particle_sys)
            {
                for (int i = 0; i < componentsInChildren.Count; i++)
                {
                    ParticleSystem particleSystem = componentsInChildren[i];
                    if (!(particleSystem == null))
                    {
                        ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
                        if (!(component == null) && component.enabled)
                        {
                            particleSystem.Stop(true);
                            component.enabled = false;
                            UnityEngine.Object.Destroy(particleSystem);
                            UnityEngine.Object.Destroy(component);
                        }
                    }
                }
                return true;
            }
            return false;
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00008CD0 File Offset: 0x00006ED0
        public static bool polygon_check(Player user, int polys)
        {
            if (polys >= GlobalUtils.max_polygons)
            {
                Il2CppArrayBase<Renderer> componentsInChildren = user.field_Private_VRCAvatarManager_0.GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < componentsInChildren.Count; i++)
                {
                    Renderer renderer = componentsInChildren[i];
                    if (!(renderer == null) && renderer.enabled)
                    {
                        renderer.enabled = false;
                        UnityEngine.Object.Destroy(renderer);
                    }
                }
                return true;
            }
            return false;
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x00008D2C File Offset: 0x00006F2C
        public static bool shader_check(Player user)
        {
            Il2CppArrayBase<Renderer> componentsInChildren = user.field_Private_VRCAvatarManager_0.GetComponentsInChildren<Renderer>(true);
            Shader shader = Shader.Find("Standard");
            bool result = false;
            for (int i = 0; i < componentsInChildren.Count; i++)
            {
                Renderer renderer = componentsInChildren[i];
                if (!(renderer == null))
                {
                    for (int j = 0; j < renderer.materials.Count; j++)
                    {
                        Material material = renderer.materials[j];
                        if (Wrapper.is_known(material.shader.name))
                        {
                            material.shader = shader;
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        // Token: 0x060000D2 RID: 210 RVA: 0x00008DC8 File Offset: 0x00006FC8
        public static bool mesh_check(Player user)
        {
            Il2CppArrayBase<MeshFilter> componentsInChildren = user.GetComponentsInChildren<MeshFilter>(true);
            if (Wrapper.get_meshes(user) >= GlobalUtils.max_meshes || Wrapper.get_skinned_meshes(user) >= GlobalUtils.max_meshes || Wrapper.get_mat_slots(user) >= GlobalUtils.max_meshes)
            {
                foreach (MeshFilter meshFilter in componentsInChildren)
                {
                    UnityEngine.Object.Destroy(meshFilter);
                }
                return true;
            }
            return false;
        }

        // Token: 0x060000D3 RID: 211 RVA: 0x00008E40 File Offset: 0x00007040
        public static bool mats_check(Player user)
        {
            Il2CppArrayBase<Material> componentsInChildren = user.GetComponentsInChildren<Material>(true);
            if (Wrapper.get_mat_slots(user) >= GlobalUtils.max_meshes)
            {
                foreach (Material material in componentsInChildren)
                {
                    UnityEngine.Object.Destroy(material);
                }
                return true;
            }
            return false;
        }

        // Token: 0x060000D4 RID: 212 RVA: 0x00008EA0 File Offset: 0x000070A0
        public static bool work_hk(Player user, int polys)
        {
            bool result;
            try
            {
                if (anticrash.shader_list.Length == 0)
                {
                    anticrash.shader_list = Wrapper.get_shader_blacklist();
                }
                if (user == null)
                {
                    result = false;
                }
                else
                {
                    bool flag = anticrash.particle_check(user);
                    bool flag2 = anticrash.polygon_check(user, polys);
                    bool flag3 = anticrash.mesh_check(user);
                    bool flag4 = anticrash.mats_check(user);
                    bool flag5 = false;
                    flag5 = anticrash.shader_check(user);
                    if (flag5)
                    {
                        MelonModLogger.Log("[!!!] nuked shaders for \"" + user.field_Private_APIUser_0.displayName.ToString() + "\"");
                    }
                    if (flag)
                    {
                        MelonModLogger.Log("[!!!] nuked particles for \"" + user.field_Private_APIUser_0.displayName.ToString() + "\"");
                    }
                    if (flag2)
                    {
                        MelonModLogger.Log("[!!!] nuked avatar for \"" + user.field_Private_APIUser_0.displayName.ToString() + "\"");
                    }
                    if (flag3)
                    {
                        MelonModLogger.Log("[!!!] nuked meshes for \"" + user.field_Private_APIUser_0.displayName.ToString() + "\"");
                    }
                    if (flag3)
                    {
                        MelonModLogger.Log("[!!!] nuked materials for \"" + user.field_Private_APIUser_0.displayName.ToString() + "\"");
                    }
                    if (flag2 || flag3 || flag || flag5 || flag4)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        // Token: 0x0400009B RID: 155
        public static Dictionary<string, avatar_data> anti_crash_list = new Dictionary<string, avatar_data>();

        // Token: 0x0400009C RID: 156
        public static string[] shader_list;

        // Token: 0x0400009D RID: 157
        public static List<string> shader_list_local = new List<string>();

        public struct avatar_data
        {
            // Token: 0x04000066 RID: 102
            public string asseturl;

            // Token: 0x04000067 RID: 103
            public int polys;
        }
    }
}
