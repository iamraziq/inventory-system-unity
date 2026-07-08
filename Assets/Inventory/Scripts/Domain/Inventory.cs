using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Unity.Mathematics;

public class Inventory
{
    private readonly ItemStack[] slots; //fixed array, null = empty
    private readonly Dictionary<Item, int> totals = new(); // O(1) query index

    public event Action InventoryChanged; // the broadcast channel

    public Inventory(int size) => slots = new ItemStack[size];

    public int Size => slots.Length;
    public ItemStack GetSlot(int i) => slots[i];

    private void NotifyChanged() => InventoryChanged?.Invoke(); // fire: no idea who listens

    //single write path - these are the ONLY places the index changes
    private void RecordAdd(Item item, int n)
        => totals[item] = (totals.TryGetValue(item, out int c) ? c : 0) + n;
    private void RecordRemove(Item item, int n)
    {
        int c = totals[item] - n;
        if(c <= 0) totals.Remove(item); else totals[item] = c;
    }

    public int CountItem(Item item) => totals.TryGetValue(item, out int n) ? n : 0; // O(1)
    public bool HasItem(Item item, int amount) => CountItem(item) >= amount;

    public int AddItem(ItemStack incoming) // two pass
    {
        int remaining = incoming.Count;
        Item item = incoming.Item;

        for(int i = 0; i < slots.Length && remaining > 0; i++) // pass 1: top up first
            if(slots[i] != null && slots[i].Item == item) // reference identity
            {
                int before = slots[i].Count;
                remaining = slots[i].AddUpTo(remaining);
                RecordAdd(item, slots[i].Count - before);
            }
        for(int i = 0; i < slots.Length && remaining > 0; i++) // pass 2: fill empties
            if(slots[i] == null)
            {
                int toPlace = Math.Min(remaining, item.MaxStackSize);
                slots[i] = new ItemStack(item, toPlace); // new object, never alias
                RecordAdd(item, toPlace);
                remaining -= toPlace;
            }
        NotifyChanged();
        return remaining;
    }

    public bool RemoveItem(Item item, int amount) // atomic
    {
        if(!HasItem(item, amount)) return false; // all or nothing gate

        int remaining = amount;
        for(int i = 0; i < slots.Length && remaining > 0; i++)
            if(slots[i] != null && slots[i].Item == item)
            {
                int take = Math.Min(remaining, slots[i].Count);
                if(take == slots[i].Count) slots[i] = null;
                else slots[i].Reduce(take);
                RecordRemove(item, take);
                remaining -= take;
            }
        NotifyChanged();
        return true;
    }

    public void MoveStack(int from, int to)
    {
        (slots[from], slots[to]) = (slots[to], slots[from]); // swap - reversible for undo later
        NotifyChanged();
    }
}
