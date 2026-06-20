using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLib
{
    public class ItemStack<TItem>
    {
        public TItem Item { get; private set; }
        public int Count { get; private set; }

        public bool IsEmpty => Item == null || Count <= 0;

        
        public void Set(TItem item, int count)
        {
            Item = item;
            Count = count;
        }

        public void Add(int amount)
        {
            Count += amount;
        }

        public int Remove(int amount)
        {
            int removed = System.Math.Min(amount, Count);
            Count -= removed;

            if (Count <= 0)
                Clear();

            return removed;
        }

        public void Clear()
        {
            Item = default;
            Count = 0;
        }
    }
}


