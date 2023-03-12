using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Items/Item")]
public class ItemObject : ScriptableObject
{
    [Space]
    [Header("Properties")]
    [Space]
    public string Name = "EQUIPMENT";
}
