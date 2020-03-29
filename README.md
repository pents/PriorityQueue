# PriorityQueue

PriorityQueue simple implementation 

[NuGet Package](https://www.nuget.org/packages/Pents.PQ)

## Usage

You can define Max/Min heap implementations 

```C#
    public class MaxHeap : PriorityQueue<int>
    {
        public MaxHeap() : base(item => item, PriorityQueueDirection.MAX)
        {
        }
    }
    
    public class MinHeap : PriorityQueue<int>
    {
        public MinHeap() : base(item => item, PriorityQueueDirection.MIN)
        {
        }
    }
```

or make your own
```C#

var priorityQueue = new PriorityQueue<Node>(item => item.Value, PriorityQueueDirection.MAX);
priorityQueue.Insert(new Node(5)); // O(log(n))
priorityQueue.Peek(); // O(1)
priorityQueue.Remove(); // O(log(n))
```
## License
[MIT](https://choosealicense.com/licenses/mit/)