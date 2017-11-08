using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ResourceType { EditorResource, AssetBundle }

public class ResourceManager : MonoBehaviour {

    private static ResourceManager instance;

    [SerializeField]
    private ResourceType resourceType = ResourceType.EditorResource;

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

#if UNITY_EDITOR
    // Build Menu
    [MenuItem("Build/AssetBundle/All Asset")]
    public static void BuildAllAssetBundles()
    {

    }

    // Build Menu
    [MenuItem("Build/AssetBundle/Target Asset")]
    public static void BuildTargetAssetBundles()
    {

    }

    // Project Windows Assets Menu
    [MenuItem("Assets/AssetBundle/Target Asset")]
    public static void ProjectBuildTargetAssetBundles()
    {

    }
#endif
}
