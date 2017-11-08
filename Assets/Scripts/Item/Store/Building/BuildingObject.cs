using UnityEngine;

public enum BuildingType
{
    None,
    House,
    Wood,
    Stone
}

[CreateAssetMenu(fileName = "New Building", menuName = "Building/BuildingObject")]
public class BuildingObject : ScriptableObject
{
    new public string name = "New Building";

    public BuildingType type = BuildingType.None;
    public Sprite icon = null;
    public GameObject buildingObject;
}
