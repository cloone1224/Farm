using UnityEngine;

public enum StoreType
{
    None,
    Warehouse,
    PlacementHouse,
    Max
}

[CreateAssetMenu(fileName = "New Store Item", menuName = "Item/StoreItem")]
public class StoreItem : Item {

    new public string name = "New Store Item";

    public StoreType type = StoreType.None;
}
