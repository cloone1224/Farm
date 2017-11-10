using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResourceManager))]
class ResourceManagerInspector : Editor
{
    private ResourceManager resourceManager = null;

    void Awake()
    {
        resourceManager = target as ResourceManager;
    }

    public override void OnInspectorGUI()
    {
        if (resourceManager == null)
            return;

        base.DrawDefaultInspector();

        EditorGUILayout.HelpBox("Build all assetbundle in this project.", MessageType.Info);
        
        AssetBundleBuilder.OutputPath = EditorGUILayout.TextField("Output Path", AssetBundleBuilder.OutputPath);

        GUI.color = Color.yellow;
        if (GUILayout.Button("Build All AssetBundle"))
        {
            AssetBundleBuilder.BuildAssetBundle();
        }
    }
}

