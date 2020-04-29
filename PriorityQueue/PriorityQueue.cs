using System;
using Pents.PQ.Abstractions;
using Pents.PQ.Enums;

namespace Pents.PQ
{
     public class PriorityQueue<T> : IPriorityQueue<T> where T: IComparable<T>
    {
        private readonly PriorityQueueDirection _direction;
        private T _removeResult;
        private T[] _collection;
        private int _lastIndex;
        
        public PriorityQueue(PriorityQueueDirection direction = PriorityQueueDirection.MIN)
        {
            _collection = new T[0];
            _lastIndex = -1;
            _direction = direction;
        }

        public T[] Sort()
        {
            var tempIndex = _lastIndex+1;
            while (_lastIndex > 0)
            {
                Swap(0, _lastIndex);
                _lastIndex--;
                NormalizeDownwards(0);    
            }

            var result = new T[tempIndex];
            Array.Copy(_collection, 0, result, 0, tempIndex);
            return result;
        }
        
        public bool IsEmpty()
        {
            return _lastIndex == -1;
        }

        public void Insert(T item)
        {
            _lastIndex++;
            if (_lastIndex >= _collection.Length)
            {
                IncreaseCollectionSize();
            }
               
            _collection[_lastIndex] = item;
            var lastElementParentIndex = ParentIndex(_lastIndex);
            NormalizeUpwards(_lastIndex, lastElementParentIndex);
        }

        public T Remove()
        {
            if (!IsEmpty())
            {
                _removeResult = _collection[0];
                Swap(0, _lastIndex);
                _collection[_lastIndex] = default;
                _lastIndex--;
                NormalizeDownwards(0);
                return _removeResult;
            }

            throw new ArgumentException("Heap is empty");
        }

        public T Peek()
        {
            return _collection[0];
        }

        private void IncreaseCollectionSize()
        {
            var newCollection = new T[(_lastIndex+5)*2];

            for (int i = 0; i < _lastIndex; i++)
            {
                newCollection[i] = _collection[i];
            }
            
            _collection = newCollection;
        }
        
        private void NormalizeUpwards(int elementIndex, int elementParentIndex)
        {
            if (_lastIndex == 0)
            {
                return;
            }
            while (true)
            {
                var element = _collection[elementIndex];

                var elementParent = _collection[elementParentIndex];
                
                if (!IsOkPlacement(element, elementParent))
                {
                    Swap(elementIndex, elementParentIndex);
                    elementIndex = elementParentIndex;
                    elementParentIndex = ParentIndex(elementParentIndex);
                    continue;
                }

                break;
            }
        }
        private void NormalizeDownwards(int parentIndex)
        {
            if (_collection.Length == 0)
                return;
            
            while (true)
            {
                var leftChildIndex = LeftIndex(parentIndex);
                var rightChildIndex = RightIndex(parentIndex);
                var parent = _collection[parentIndex];
                var childIndex = DirectionalOkChildIndex(leftChildIndex, rightChildIndex);
                if (childIndex != -1)
                {
                    var child = _collection[childIndex];
                    if (!IsOkPlacement(child, parent))
                    {
                        Swap(parentIndex, childIndex);
                        NormalizeUpwards(parentIndex, ParentIndex(parentIndex));
                        parentIndex = childIndex;
                        continue;
                    }
                }

                break;
            }
        }
        private int DirectionalOkChildIndex(int leftChildIndex, int rightChildIndex)
        {
            var leftChildExists = _lastIndex >= leftChildIndex;
            var rightChildExists = _lastIndex >= rightChildIndex;

            var leftChildValue = leftChildExists ? _collection[leftChildIndex] : default;
            var rightChildValue = rightChildExists ? _collection[rightChildIndex] : default;
            
            if (leftChildExists && rightChildExists)
            {
                var comparer = leftChildValue.CompareTo(rightChildValue);
                switch (_direction)
                {
                    case PriorityQueueDirection.MIN:
                        return comparer < 0 ? leftChildIndex : rightChildIndex;
                    case PriorityQueueDirection.MAX:
                        return comparer > 0 ? leftChildIndex : rightChildIndex;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_direction));
                }
            }

            if (leftChildExists)
            {
                return leftChildIndex;
            }

            if (rightChildExists)
            {
                return rightChildIndex;
            }

            return -1;
        }
        private bool IsOkPlacement(T weightChild, T weightParent)
        {
            var comparerResult = weightChild.CompareTo(weightParent);
            if (comparerResult == 0) return true;
            switch (_direction)
            {
                case PriorityQueueDirection.MAX:
                    return comparerResult < 0;
                case PriorityQueueDirection.MIN:
                    return comparerResult > 0;
                default:
                    throw new ArgumentException(nameof(_direction));
            }
        }
        private void Swap(int index1, int index2) => (_collection[index1], _collection[index2]) = (_collection[index2], _collection[index1]);
        private int ParentIndex(int itemIndex) => (itemIndex-1) / 2;
        private int LeftIndex(int itemIndex) => (2 * itemIndex) + 1;
        private int RightIndex(int itemIndex) => (2 * itemIndex) + 2;
    }
}