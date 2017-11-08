using UnityEngine;

public enum InventoryType
{
    None,

    Max
}

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Item/InventoryItem")]
public class InventoryItem : Item {

    new public string name = "New Inventory Item";

    public InventoryType type = InventoryType.None;
}
