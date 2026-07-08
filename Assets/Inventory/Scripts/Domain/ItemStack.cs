using System;

public class ItemStack
{
    public Item Item { get;  } // immutable: which item never changes
    public int Count { get; private set; } // mutable, but only through cocntrolled doors

    public ItemStack(Item item, int count)
    {
        if(item == null) throw new ArgumentNullException(nameof(item));
        if(count < 1 || count > item.MaxStackSize)
            throw new ArgumentOutOfRangeException(nameof(count));
        Item = item;
        Count = count;
    }

    public int AddUpTo(int amount) // adds what fits, retuens the leftover
    {
        int space = Item.MaxStackSize - Count;
        int added = Math.Min(space, amount);
        Count += added; // need to add what fits
        return amount - added;
    }

    public void Reduce(int n) // guards its invariant: never drops below 1
    {
        if(n < 1 || n >= Count) throw new ArgumentOutOfRangeException(nameof(n));
        Count -= n;
    }
}
