using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Inventory/Food")]
public class FoodItem : Item
{
    [SerializeField] private int hungerRestored = 4;
    public int HungerRestored => hungerRestored;
    public override void Use() => Debug.Log($"Ate {Name}, +{hungerRestored} hunger.");
}
