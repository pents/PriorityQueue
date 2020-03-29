using System;
using PQ.Abstractions;
using PriorityQueue.Enums;

namespace PriorityQueue
{
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        private readonly PriorityQueueDirection _direction;
        private readonly Func<T, int> _sortingRule;
        private T _removeResult = default;
        private T[] _collection = new T[0];
        private int _lastIndex = -1;
        
        public PriorityQueue(Func<T, int> sortingRule, PriorityQueueDirection direction = PriorityQueueDirection.MIN)
        {
            _sortingRule = sortingRule;
            _direction = direction;
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
            _removeResult = _collection[0];
            Swap(0, _lastIndex);
            _lastIndex--;
            NormalizeDownwards(0);
            return _removeResult;
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
            if (_lastIndex == 1)
            {
                return;
            }
            while (true)
            {
                var element = _collection[elementIndex];

                var elementParent = _collection[elementParentIndex];

                var elementWeight = _sortingRule(element);
                var elementParentWeight = _sortingRule(elementParent);
                if (!IsOkPlacement(elementWeight, elementParentWeight))
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
            while (true)
            {
                var leftChildIndex = LeftIndex(parentIndex);
                var rightChildIndex = RightIndex(parentIndex);
                var parent = _sortingRule(_collection[parentIndex]);
                var childIndex = DirectionalOkChildIndex(leftChildIndex, rightChildIndex);
                if (childIndex != int.MinValue)
                {
                    var child = _sortingRule(_collection[childIndex]);
                    if (!IsOkPlacement(child, parent))
                    {
                        Swap(parentIndex, childIndex);
                        parentIndex = childIndex;
                        continue;
                    }
                }

                break;
            }
        }
        private int DirectionalOkChildIndex(int leftChildIndex, int rightChildIndex)
        {
            var leftChildValue = _lastIndex > leftChildIndex ? _sortingRule(_collection[leftChildIndex]) : int.MinValue;
            var rightChildValue = _lastIndex > rightChildIndex ? _sortingRule(_collection[rightChildIndex]) : int.MinValue;
            
            if (leftChildValue != int.MinValue && rightChildValue != int.MinValue)
            {
                return _direction switch
                {
                    PriorityQueueDirection.MIN => leftChildValue > rightChildValue ? rightChildIndex : leftChildIndex,
                    PriorityQueueDirection.MAX => leftChildValue < rightChildValue ? leftChildIndex : rightChildIndex,
                    _                          => throw new ArgumentOutOfRangeException(nameof(_direction))
                };
            }

            if (leftChildValue != int.MinValue && rightChildValue == int.MinValue)
            {
                return leftChildIndex;
            }

            if (rightChildValue != int.MinValue && leftChildValue == int.MinValue)
            {
                return rightChildIndex;
            }

            return int.MinValue;
        }
        private bool IsOkPlacement(int weightChild, int weightParent)
        {
            if (weightChild == weightParent) return true;
            return _direction switch
            {
                PriorityQueueDirection.MIN => (weightParent < weightChild),
                PriorityQueueDirection.MAX => (weightParent > weightChild),
                _                          => throw new ArgumentException(nameof(_direction))
            };
        }
        private void Swap(int index1, int index2) => (_collection[index1], _collection[index2]) = (_collection[index2], _collection[index1]);
        private int ParentIndex(int itemIndex) => (itemIndex-1) / 2;
        private int LeftIndex(int itemIndex) => (2 * itemIndex) + 1;
        private int RightIndex(int itemIndex) => (2 * itemIndex) + 2;
    }
}