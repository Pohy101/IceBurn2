using Il2CppSystem.Collections.Generic;

namespace IceBurn.Other
{
    // Token: 0x02000003 RID: 3
    public static class ListExtensions
    {
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
