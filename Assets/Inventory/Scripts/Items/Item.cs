using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public abstract class Item : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField, Min(1)] private int maxStackSize = 64;
    [SerializeField] private Sprite icon;

    public string Name => displayName;
    public int MaxStackSize => maxStackSize;
    public Sprite Icon => icon;

    public abstract void Use();
}
