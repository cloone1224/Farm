using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    None,
    House,
    Wood,
    Stone
}

public class BuildingBase : MonoBehaviour
{
    [SerializeField]
    private BuildingType buildType = BuildingType.None;

    [SerializeField]
    private GameObject buildingObject;

    protected int buildingCost = 0;

    public virtual void Build()
    {
        Debug.Log(name + " builded.");
    }
}

public class BuildingObject : BuildingBase
{
    public override void Build()
    {
        base.Build();
    }
}
