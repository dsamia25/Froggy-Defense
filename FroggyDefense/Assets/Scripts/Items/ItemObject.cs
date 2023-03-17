using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Items/Item")]
public class ItemObject : ScriptableObject
{
    [Space]
    [Header("Properties")]
    [Space]
    public string Name = "ITEM";
    public string Description = "A NEW ITEM";
    public bool IsStackable { get; } = false;

    public Sprite Icon = null;
}
