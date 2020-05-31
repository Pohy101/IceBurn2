using System;
using Il2CppSystem.Collections.Generic;

namespace IceBurn.Mod
{
	// Token: 0x02000003 RID: 3
	public static class ListExtensions
	{
		// Token: 0x0600001F RID: 31 RVA: 0x0000354C File Offset: 0x0000174C
		public static T[] ToArrayExtension<T>(this List<T> list)
		{
			T[] array = new T[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = list[i];
			}
			return array;
		}
	}
}
