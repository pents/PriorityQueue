namespace Pents.PQ.Abstractions
{
    public interface IPriorityQueue<T>
    {
        bool IsEmpty();
        void Insert(T item);
        T Remove();
        T Peek();
    }
}