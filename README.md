# PriorityQueue

PriorityQueue simple implementation 

[NuGet Package](https://www.nuget.org/packages/Pents.PQ)

## Usage

You can define Max/Min heap implementations 

```C#
    public class MaxHeap : PriorityQueue<int>
    {
        public MaxHeap() : base(PriorityQueueDirection.MAX)
        {
        }
    }
    
    public class MinHeap : PriorityQueue<int>
    {
        public MinHeap() : base(PriorityQueueDirection.MIN)
        {
        }
    }
```

or make your own

```C#
var priorityQueue = new PriorityQueue<Node>(PriorityQueueDirection.MAX);
priorityQueue.Insert(new Node(5)); // O(log(n))
priorityQueue.Peek(); // O(1)
priorityQueue.Remove(); // O(log(n))
```

or use it for sorting

```C#
var array = new int[] {0, -9, 5, 2, 7, 1, 8};

var sortedArray = array.HeapSort();

// Output: -9 0 1 2 5 7 8

```

note, that custom type needs to implement IComparable to work with this



## License
[MIT](https://choosealicense.com/licenses/mit/)