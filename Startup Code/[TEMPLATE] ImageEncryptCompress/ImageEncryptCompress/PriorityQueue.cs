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
        private List<HuffmanNode> list;
        public int Count { get { return list.Count; } }
        public readonly bool IsDescending;

        public PriorityQueue() // O(1)
        {
            list = new List<HuffmanNode>();
        }

        public PriorityQueue(bool isdesc) // O(1)
            : this()
        {
            IsDescending = isdesc;
        }

        public PriorityQueue(int capacity) // O(n)
            : this(capacity, false)
        { }

        public PriorityQueue(IEnumerable<HuffmanNode> collection) // O(n)
            : this(collection, false)
        { }

        public PriorityQueue(int capacity, bool isdesc) // O(n)
        {
            list = new List<HuffmanNode>(capacity);
            IsDescending = isdesc;
        }

        public PriorityQueue(IEnumerable<HuffmanNode> collection, bool isdesc) // O(n)
            : this()
        {
            IsDescending = isdesc;
            foreach (var item in collection)
                Push(item);
        }

        public void Push(HuffmanNode x) // O(log n)
        {
            list.Add(x);
            int i = Count - 1;

            while (i > 0)
            {
                int p = (i - 1) / 2;
                if ((IsDescending ? -1 : 1) * list[p].Frequency.CompareTo(x.Frequency) <= 0) break;

                list[i] = list[p];
                i = p;
            }

            if (Count > 0) list[i] = x;
        }

        public HuffmanNode Pop() // O(log n)
        {
            HuffmanNode target = Top();
            HuffmanNode root = list[Count - 1];
            list.RemoveAt(Count - 1);

            int i = 0;
            while (i * 2 + 1 < Count)
            {
                int a = i * 2 + 1;
                int b = i * 2 + 2;
                int c = b < Count && (IsDescending ? -1 : 1) * list[b].Frequency.CompareTo(list[a].Frequency) < 0 ? b : a;

                if ((IsDescending ? -1 : 1) * list[c].Frequency.CompareTo(root.Frequency) >= 0) break;
                list[i] = list[c];
                i = c;
            }

            if (Count > 0) list[i] = root;
            return target;
        }

        public HuffmanNode Top() // O(1)
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return list[0];
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}
