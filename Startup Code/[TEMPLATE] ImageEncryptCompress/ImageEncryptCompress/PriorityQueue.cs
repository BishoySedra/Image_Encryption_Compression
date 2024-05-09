using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptCompress
{

    public class HuffmanNode
    {
        public int Frequency { get; set; } 
        public int Pixel { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
    }
    public class PriorityQueue
    {
        private List<HuffmanNode> list; // O(1)
        public int Count { get { return list.Count; } } // O(1)
        public readonly bool IsDescending; // O(1)

        public PriorityQueue() // O(1)
        {
            list = new List<HuffmanNode>(); // O(1)
        }

        public PriorityQueue(bool isdesc) // O(1)
            : this()
        {
            IsDescending = isdesc; // O(1)
        }

        public PriorityQueue(int capacity) // O(n)
            : this(capacity, false) // O(n)
        { }

        public PriorityQueue(IEnumerable<HuffmanNode> collection) // O(n)
            : this(collection, false) // O(n)
        { }

        public PriorityQueue(int capacity, bool isdesc) // O(n)
        {
            list = new List<HuffmanNode>(capacity); // O(n)
            IsDescending = isdesc; // O(1)
        }

        public PriorityQueue(IEnumerable<HuffmanNode> collection, bool isdesc) // O(n)
            : this()
        {
            IsDescending = isdesc; // O(1)
            // O(n log n)
            foreach (var item in collection)  // O(n)
            { 
                Push(item); // O(log n)
            }
        }

        // O(log n)
        public void Push(HuffmanNode x)
        {
            list.Add(x); // O(1)
            int i = Count - 1; // O(1)

            while (i > 0) // O(log n)
            {
                int p = (i - 1) / 2; // O(1)
                if ((IsDescending ? -1 : 1) * list[p].Frequency.CompareTo(x.Frequency) <= 0) // O(1)
                {
                    break; // O(1)
                } 

                list[i] = list[p]; // O(1)
                i = p; // O(1)
            }

            if (Count > 0) // O(1)
            {  
                list[i] = x; // O(1)
            }
        }

        public HuffmanNode Pop() // O(log n)
        {
            HuffmanNode target = Top(); // O(1)
            HuffmanNode root = list[Count - 1]; // O(1)
            list.RemoveAt(Count - 1); // O(1)

            int i = 0; // O(1)
            while (i * 2 + 1 < Count)
            {
                int a = i * 2 + 1; // O(1)
                int b = i * 2 + 2; // O(1)
                int c = b < Count && (IsDescending ? -1 : 1) * list[b].Frequency.CompareTo(list[a].Frequency) < 0 ? b : a; // O(1)

                // O(1)
                if ((IsDescending ? -1 : 1) * list[c].Frequency.CompareTo(root.Frequency) >= 0) {
                    break; // O(1)
                }

                list[i] = list[c]; // O(1)
                i = c; // O(1)
            }

            // O(1)
            if (Count > 0) {
                list[i] = root; // O(1)
            }
            return target; // O(1)
        }

        public HuffmanNode Top() // O(1)
        {
            // O(1)
            if (Count == 0) { 
                throw new InvalidOperationException("Queue is empty."); // O(1)
            }
            return list[0]; // O(1)
        }

        // O(1)
        public void Clear()
        {
            list.Clear(); // O(1)
        }
    }
}
