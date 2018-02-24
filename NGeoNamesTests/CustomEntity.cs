using System.Collections;
using System.Collections.Generic;
namespace NGeoNamesTests
{
    internal class CustomEntity
    {
        public string[] Data { get; set; }
    }

    internal class CustomEntityComparer : IComparer<CustomEntity>, IComparer
    {
        public int Compare(CustomEntity x, CustomEntity y)
        {
            int r = x.Data.Length.CompareTo(y.Data.Length);
            if (r != 0)
                return r;

            for (int i = 0; i < x.Data.Length && r == 0; i++)
                r = x.Data[i].CompareTo(y.Data[i]);
            return r;
        }

        public int Compare(object x, object y)
        {
            return Compare(x as CustomEntity, y as CustomEntity);
        }
    }
}
