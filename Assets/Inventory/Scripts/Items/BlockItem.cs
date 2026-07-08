using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Inventory/Block")]
public class BlockItem : Item
{
    public override void Use() => Debug.Log($"Placed a {Name}.");
}
