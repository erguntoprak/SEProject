using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.Business.Helpers
{
    public static class RandomHelper
    {
        private static Random Rand = null;

        public static List<T> RandomList<T>(
            this T[] list, int num_values)
        {
            if (Rand == null) Rand = new Random();

            if (num_values >= list.Length)
                num_values = list.Length;

            List<T> tmpList = new List<T>(list);
            List<T> newList = new List<T>();

            while (newList.Count < num_values && tmpList.Count > 0)
            {
                int index = Rand.Next(0, tmpList.Count);
                newList.Add(tmpList[index]);
                tmpList.RemoveAt(index);
            }
            return newList;
        }
    }
}
