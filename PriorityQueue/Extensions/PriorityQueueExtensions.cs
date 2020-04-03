using System;
using Pents.PQ.Enums;

namespace Pents.PQ.Extensions
{
    public static class PriorityQueueExtensions
    {
        public static T[] HeapSort<T>(this T[] array) where T : IComparable
        {
            var heap = new PriorityQueue<T>(PriorityQueueDirection.MAX);
            for (var i = 0; i < array.Length; i++)
            {
                heap.Insert(array[i]);
            }
            return heap.Sort();
        }
    }

}