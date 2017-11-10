using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

using System.Collections.Generic;

public enum ResourceType { EditorResource, AssetBundle }
public enum AssetType
{
    None,

    UI,
    Table,
    Scene,
    Sound,
    Movie,

    Max
}

public class ResourceManager : MonoBehaviour {

    #region Singleton
    private static ResourceManager instance;
    public static ResourceManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ResourceManager");
            DestroyImmediate(this);
            return;
        }

        instance = this;
    }
    #endregion
    
    [SerializeField]
    private ResourceType resourceType = ResourceType.EditorResource;
    public ResourceType ResourceType { get { return resourceType; } }
    
    [SerializeField]
    private bool removeAssetBundleManifest = false;
    public bool RemoveAssetBundleManifest { get { return removeAssetBundleManifest; } }    
}
