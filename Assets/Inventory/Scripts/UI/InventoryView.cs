using UnityEngine;
using System.Collections.Generic;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private SlotView slotPrefab;
    [SerializeField] private Transform slotParent; // the InventoryPanel object

    private Inventory inventory;
    private readonly List<SlotView> slotViews = new();
    private readonly CommandProcessor commands = new();

    public void Bind(Inventory inventory)
    {
        this.inventory = inventory;

        for(int i = 0; i < inventory.Size; i++) // spawn one SlotView per slot
        {
            SlotView view = Instantiate(slotPrefab, slotParent);
            view.SetIndex(i);
            view.Clicked += OnSlotClicked; // listen to each slot
            view.Moved += OnSlotMoved;
            slotViews.Add(view);
        }

        inventory.InventoryChanged += Redraw; // Observer: subscribe
        Redraw();                             // draw the initial state once
    }

    private void OnSlotClicked(int index)
    {
        ItemStack stack = inventory.GetSlot(index);
        if(stack == null) return; // empty slot - nothing to use

        stack.Item.Use(); // polymorphism: dirt logs  "Placed", apple logs "Ate"
        inventory.ConsumeAt(index); // consume one -> event fires -> Redraw runs automatically
    }

    private void OnSlotMoved(int from, int to)
    {
        commands.Do(new MoveCommand(inventory, from, to)); // NOT inventory.MoveStack directly!
    }

    public void UndoLast() => commands.UndoLast(); // the Undo button calls this

    private void Redraw()
    {
        for(int i = 0; i < slotViews.Count; i++)
            slotViews[i].Render(inventory.GetSlot(i)); // push each slot's state to its view
    }

    private void OnDestroy()
    {
        if(inventory != null)
            inventory.InventoryChanged -= Redraw; // always unsubscribe
    }
}
