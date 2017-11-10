using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class AssetBundleBuilder
{
    public static string _assetBundleFolderName = "AssetBundle";
    public static string _assetBundleManifastFileExtension = "*.manifest";

    // LZMA 방식으로 압축률은 좋으나, 로딩이 좀 느리다.
    //private static BuildAssetBundleOptions _buildAssetBundleOptions = BuildAssetBundleOptions.None;
    // LZ4 방식으로 압축률은 LZMA에 비해 떨어지나, 로딩시 좀 빠르다.
    private static BuildAssetBundleOptions _buildAssetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle;
    
    // 지정이 안 되어 있으면 기본 AssetBundle/타켓경로로 지정된다.
    private static string _outputPath;

    public static string OutputPath
    {
        get
        {
            if (string.IsNullOrEmpty(_outputPath))
                return GetDefaultOutputPath();

            return _outputPath;
        }
        set
        {
            _outputPath = value;
        }
    }

    public static string FullOutputPath
    {
        get
        {
            return Application.dataPath + "/" + OutputPath;
        }
    }

    private static string GetDefaultOutputPath()
    {
        return _assetBundleFolderName + "/" + Build.GetPlatformName();
    }

    public static void BuildAssetBundle()
    {
        try
        {
            if (ResourceManager.Instance.RemoveAssetBundleManifest)
            {
                DeleteAssetBundleManifaseFile();
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
    
    // Project Windows Assets Menu
    [MenuItem("Assets/AssetBundle/Target Asset")]
    public static void BuildTargetAssetBundles()
    {
        UnityEngine.Object[] selectedObjects = Selection.objects;

        if (selectedObjects == null)
            return;

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            UnityEngine.Object obj = selectedObjects[i];
            if (obj == null)
                continue;

            string assetBundlePath = AssetDatabase.GetAssetPath(obj.GetInstanceID());

            bool? isDirectory = IsDirectory(assetBundlePath);
            if (isDirectory == null)
                continue;

            if (isDirectory.Value)
            {
                BuildSelectFolderAssetBundle(assetBundlePath);
                continue;
            }
            
            BuildSelectFileAssetBundle(assetBundlePath);
        }
    }

    private static void BuildSelectFolderAssetBundle(string assetBundlePath)
    {
        if (string.IsNullOrEmpty(assetBundlePath))
            return;

        string[] buildAssetList = null;

        // [TODO] 폴더내의 항목을 수집

        if (Build._excludeAssetBundleExtension != null && Build._excludeAssetBundleExtension.Length > 0)
        {
            buildAssetList = buildAssetList.Where(x => Build._excludeAssetBundleExtension.Contains(Path.GetExtension(x)) == false).ToArray();
        }

        BuildAssetBundle(buildAssetList);
    }

    private static void BuildSelectFileAssetBundle(string assetBundlePath)
    {
        if (string.IsNullOrEmpty(assetBundlePath))
            return;

        string[] buildAssetList = AssetDatabase.GetDependencies(assetBundlePath);
        
        if (Build._excludeAssetBundleExtension != null && Build._excludeAssetBundleExtension.Length > 0)
        {
            buildAssetList = buildAssetList.Where(x => Build._excludeAssetBundleExtension.Contains(Path.GetExtension(x)) == false).ToArray();
        }

        BuildAssetBundle(buildAssetList);
    }

    private static void BuildAssetBundle(string[] assetList)
    {
        EditorUtility.ClearProgressBar();

        if (assetList == null || assetList.Length == 0)
            return;

        // Clear
        ClearAssetBundleName();

        string assetBundlePath = string.Format("{0}/{1}", _assetBundleFolderName, Build.GetPlatformName());

        if (!Directory.Exists(assetBundlePath))
            Directory.CreateDirectory(assetBundlePath);

        EditorUtility.DisplayProgressBar("Build AssetBundle", "에셋번들을 생성 중입니다.", 0.0f);

        BuildAssetBundleFromAssetBundleBuild(assetList, assetBundlePath);

        EditorUtility.DisplayProgressBar("Build AssetBundle", "에셋번들을 생성 중입니다.", 1.0f);
        EditorUtility.ClearProgressBar();

        // Clear
        ClearAssetBundleName();
    }

    private static void BuildAssetBundleFromAssetBundleBuild(string[] assetList, string assetBundlePath)
    {
        AssetBundleBuild[] assetBundleBuildList = new AssetBundleBuild[assetList.Length];

        for (int i = 0; i < assetList.Length; i++)
        {
            assetBundleBuildList[i] = new AssetBundleBuild();
            assetBundleBuildList[i].assetBundleName = string.Format("{0}{1}", assetList[i], Build._assetBundleExtension);
            assetBundleBuildList[i].assetNames = new[] { string.Format("{0}", assetList[i]) };
        }

        BuildPipeline.BuildAssetBundles(assetBundlePath, assetBundleBuildList, _buildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
    }

    private static void ClearAssetBundleName()
    {
        EditorUtility.DisplayProgressBar("Build AssetBundle", "모든 에셋의 번들이름을 초기화 하는 중 입니다.", 0.0f);

        AssetDatabase.RemoveUnusedAssetBundleNames();

        string[] allAssetBundle = AssetDatabase.GetAllAssetBundleNames();

        EditorUtility.DisplayProgressBar("Build AssetBundle", "모든 에셋의 번들이름을 초기화 하는 중 입니다.", 0.2f);

        for (int i = 0; i < allAssetBundle.Length; ++i)
        {
            string bundleName = allAssetBundle[i];
            AssetDatabase.RemoveAssetBundleName(bundleName, true);

            EditorUtility.DisplayProgressBar("Build AssetBundle", "모든 에셋의 번들이름을 초기화 하는 중 입니다.", (float)i / (float)allAssetBundle.Length);
        }

        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
    }

    public static void DeleteAssetBundleManifaseFile()
    {
        EditorUtility.DisplayProgressBar("Build AssetBundle", "에셋 번들의 manifase 파일을 삭제 중 입니다.", 0.0f);

        string assetBundlePath = string.Format("{0}{1}/{2}/{3}", Application.dataPath.Replace("Assets", ""), _assetBundleFolderName, Build.GetPlatformName(), "assets");

        DirectoryInfo info = new DirectoryInfo(assetBundlePath);
        FileInfo[] files = info.GetFiles(_assetBundleManifastFileExtension, SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; ++i)
        {
            EditorUtility.DisplayProgressBar("Build AssetBundle", "에셋 번들의 manifase 파일을 삭제 중 입니다.", (float)i / (float)files.Length);
            files[i].Delete();
        }

        EditorUtility.DisplayProgressBar("Build AssetBundle", "에셋 번들의 manifase 파일을 삭제 중 입니다.", 1.0f);
        EditorUtility.ClearProgressBar();
    }

    private static bool? IsDirectory(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        if (Directory.Exists(path))
            return true;

        if (File.Exists(path))
            return false;

        return null;
    }
}
#endif
