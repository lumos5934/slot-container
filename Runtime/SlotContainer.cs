using System;
using System.Collections.Generic;
using System.Linq;

namespace LLib
{
    public class SlotContainer<TItem, TKey> where TItem : IItem<TKey>
    {
        public enum SlotLayout
        {
            Fixed,
            Compact
        }

        private readonly List<ItemStack<TItem>> _slots;
        private readonly int _capacity;
        private readonly HashSet<int> _dirty = new();

        protected readonly SlotLayout _layout;

        public IReadOnlyList<ItemStack<TItem>> Slots => _slots;
        public bool IsUnlimited => _capacity < 0;
        public int Capacity => _capacity;
        public int Count => _slots.Count;

        public SlotContainer(SlotLayout layout, int capacity = -1)
        {
            _layout = layout;
            _capacity = capacity;
            _slots = new List<ItemStack<TItem>>();

            if (!IsUnlimited)
            {
                for (int i = 0; i < capacity; i++)
                {
                    _slots.Add(CreateSlot());
                }
            }
        }

        protected virtual ItemStack<TItem> CreateSlot()
        {
            return new ItemStack<TItem>();
        }

        public ItemStack<TItem> GetSlot(int index)
        {
            return _slots[index];
        }

        public virtual int Add(TItem item, int amount)
        {
            return OnAdd(item, amount);
        }

        public virtual int RemoveAt(int index, int amount)
        {
            return OnRemoveAt(index, amount);
        }

        public void Clear()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].Clear();
                _dirty.Add(i);
                OnSlotChanged(i, _slots[i]);
            }
        }

        public int[] ConsumeDirty()
        {
            var arr = _dirty.ToArray();
            _dirty.Clear();
            return arr;
        }

        protected virtual int OnAdd(TItem item, int amount)
        {
            int remaining = amount;

            for (int i = 0; i < _slots.Count; i++)
            {
                if (remaining <= 0)
                    break;

                var slot = _slots[i];

                if (!slot.IsEmpty && IsSameItem(slot.Item, item))
                {
                    int canAdd = Math.Min(
                        remaining,
                        slot.Item.MaxStack - slot.Count
                    );

                    if (canAdd > 0)
                    {
                        slot.Add(canAdd);
                        remaining -= canAdd;

                        _dirty.Add(i);
                        OnSlotChanged(i, slot);
                    }
                }
            }

            if (_layout == SlotLayout.Fixed)
            {
                for (int i = 0; i < _slots.Count; i++)
                {
                    if (remaining <= 0)
                        break;

                    var slot = _slots[i];

                    if (slot.IsEmpty)
                    {
                        int canAdd = Math.Min(remaining, item.MaxStack);
                        slot.Set(item, canAdd);
                        remaining -= canAdd;

                        _dirty.Add(i);
                        OnSlotChanged(i, slot);
                    }
                }
            }

            if (_layout == SlotLayout.Compact || IsUnlimited)
            {
                while (remaining > 0)
                {
                    var newSlot = CreateSlot();

                    int canAdd = Math.Min(remaining, item.MaxStack);
                    newSlot.Set(item, canAdd);

                    _slots.Add(newSlot);
                    remaining -= canAdd;

                    int index = _slots.Count - 1;
                    _dirty.Add(index);
                    OnSlotChanged(index, newSlot);

                    if (!IsUnlimited && _slots.Count >= _capacity)
                        break;
                }
            }

            return amount - remaining;
        }

        protected virtual int OnRemoveAt(int index, int amount)
        {
            var slot = _slots[index];

            if (slot.IsEmpty)
                return 0;

            int removed = slot.Remove(amount);

            _dirty.Add(index);
            OnSlotChanged(index, slot);

            if (_layout == SlotLayout.Compact && slot.IsEmpty)
            {
                _slots.RemoveAt(index);

                for (int i = index; i < _slots.Count; i++)
                {
                    _dirty.Add(i);
                    OnSlotChanged(i, _slots[i]);
                }
            }

            return removed;
        }

        protected virtual bool IsSameItem(TItem a, TItem b)
        {
            return EqualityComparer<TKey>.Default.Equals(a.Key, b.Key);
        }

        protected virtual void OnSlotChanged(int index, ItemStack<TItem> slot) { }
    }
}