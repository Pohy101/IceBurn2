using Il2CppSystem;
using System.Linq;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.SDKBase;

namespace IceBurn.Utils
{
    class GlobalUtils
    {
        // нужные переменные сюда
        public static bool Fly = false;
        public static bool ESP = false;
        public static bool FakeNamePlate = false;
        public static int flySpeed = 5;
        public static float brightness = 1f;
        public static int walkSpeed = 2;
        public static int cameraFOV = 65;
        public static Vector3 Gravity = Physics.gravity;

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

	public struct HSBColor
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00005406 File Offset: 0x00003606
		public HSBColor(float h, float s, float b, float a)
		{
			this.h = h;
			this.s = s;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00005425 File Offset: 0x00003625
		public HSBColor(float h, float s, float b)
		{
			this.h = h;
			this.s = s;
			this.b = b;
			this.a = 1f;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00005448 File Offset: 0x00003648
		public HSBColor(Color col)
		{
			HSBColor hsbcolor = HSBColor.FromColor(col);
			this.h = hsbcolor.h;
			this.s = hsbcolor.s;
			this.b = hsbcolor.b;
			this.a = hsbcolor.a;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000548C File Offset: 0x0000368C
		public static HSBColor FromColor(Color color)
		{
			HSBColor hsbcolor = new HSBColor(0f, 0f, 0f, color.a);
			float r = color.r;
			float g = color.g;
			float num = color.b;
			float num2 = Mathf.Max(r, Mathf.Max(g, num));
			if (num2 <= 0f)
			{
				return hsbcolor;
			}
			float num3 = Mathf.Min(r, Mathf.Min(g, num));
			float num4 = num2 - num3;
			if (num2 > num3)
			{
				if (g == num2)
				{
					hsbcolor.h = (num - r) / num4 * 60f + 120f;
				}
				else if (num == num2)
				{
					hsbcolor.h = (r - g) / num4 * 60f + 240f;
				}
				else if (num > g)
				{
					hsbcolor.h = (g - num) / num4 * 60f + 360f;
				}
				else
				{
					hsbcolor.h = (g - num) / num4 * 60f;
				}
				if (hsbcolor.h < 0f)
				{
					hsbcolor.h += 360f;
				}
			}
			else
			{
				hsbcolor.h = 0f;
			}
			hsbcolor.h *= 0.0027777778f;
			hsbcolor.s = num4 / num2 * 1f;
			hsbcolor.b = num2;
			return hsbcolor;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000055D0 File Offset: 0x000037D0
		public static Color ToColor(HSBColor hsbColor)
		{
			float num = hsbColor.b;
			float num2 = hsbColor.b;
			float num3 = hsbColor.b;
			if (hsbColor.s != 0f)
			{
				float num4 = hsbColor.b;
				float num5 = hsbColor.b * hsbColor.s;
				float num6 = hsbColor.b - num5;
				float num7 = hsbColor.h * 360f;
				if (num7 < 60f)
				{
					num = num4;
					num2 = num7 * num5 / 60f + num6;
					num3 = num6;
				}
				else if (num7 < 120f)
				{
					num = -(num7 - 120f) * num5 / 60f + num6;
					num2 = num4;
					num3 = num6;
				}
				else if (num7 < 180f)
				{
					num = num6;
					num2 = num4;
					num3 = (num7 - 120f) * num5 / 60f + num6;
				}
				else if (num7 < 240f)
				{
					num = num6;
					num2 = -(num7 - 240f) * num5 / 60f + num6;
					num3 = num4;
				}
				else if (num7 < 300f)
				{
					num = (num7 - 240f) * num5 / 60f + num6;
					num2 = num6;
					num3 = num4;
				}
				else if (num7 <= 360f)
				{
					num = num4;
					num2 = num6;
					num3 = -(num7 - 360f) * num5 / 60f + num6;
				}
				else
				{
					num = 0f;
					num2 = 0f;
					num3 = 0f;
				}
			}
			return new Color(Mathf.Clamp01(num), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00005744 File Offset: 0x00003944
		public Color ToColor()
		{
			return HSBColor.ToColor(this);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005754 File Offset: 0x00003954
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"H:",
				this.h.ToString(),
				" S:",
				this.s.ToString(),
				" B:",
				this.b.ToString()
			});
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000057B0 File Offset: 0x000039B0
		public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
		{
			float num;
			float num2;
			if (a.b == 0f)
			{
				num = b.h;
				num2 = b.s;
			}
			else if (b.b == 0f)
			{
				num = a.h;
				num2 = a.s;
			}
			else
			{
				if (a.s == 0f)
				{
					num = b.h;
				}
				else if (b.s == 0f)
				{
					num = a.h;
				}
				else
				{
					float num3;
					for (num3 = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t); num3 < 0f; num3 += 360f)
					{
					}
					while (num3 > 360f)
					{
						num3 -= 360f;
					}
					num = num3 / 360f;
				}
				num2 = Mathf.Lerp(a.s, b.s, t);
			}
			return new HSBColor(num, num2, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
		}

		// Token: 0x04000062 RID: 98
		public float h;

		// Token: 0x04000063 RID: 99
		public float s;

		// Token: 0x04000064 RID: 100
		public float b;

		// Token: 0x04000065 RID: 101
		public float a;
	}
}
