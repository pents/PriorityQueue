using System;
using Pents.PQ.Enums;

namespace Pents.PQ.Extensions
{
    public static class PriorityQueueExtensions
    {
        public static void HeapSort<T>(this T[] array) where T : IComparable
        {
            var heap = new PriorityQueue<T>(array, PriorityQueueDirection.MAX);
            heap.Sort();
        }
    }
}