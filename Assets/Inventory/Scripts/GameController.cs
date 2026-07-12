using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Item dirt; // dray your Dirt asset here
    [SerializeField] private Item apple; // drag your Apple asset here
    [SerializeField] private InventoryView inventoryView; 

    private Inventory inventory;

    void Start()
    {
        inventory = new Inventory(9);
        inventoryView.Bind(inventory); // UI subscribes here - model untouched

        inventory.AddItem(new ItemStack(dirt, 50));
        inventory.AddItem(new ItemStack(dirt, 30)); // watch 50 -> 64, with 16 overflowing
        inventory.AddItem(new ItemStack(apple, 5));

        Debug.Log($"Dirt total (0(1) lookup): {inventory.CountItem(dirt)}");
        Debug.Log($"Try remove 100 dirt: {inventory.RemoveItem(dirt, 100)}(atomic - refused)");
    }

    private void LogContents() // Dummy code for testing
    {
        string line = "";
        for(int i = 0; i < inventory.Size; i++)
        {
            var s = inventory.GetSlot(i);
            if(s != null) line += $"[{i}: {s.Item.Name} x{s.Count}]";
        }

        Debug.Log(line == "" ? "(empty)" : line);
    }
}
