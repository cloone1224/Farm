using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Build
{
    public const string _assetBundleBasePath = "AssetBundle/";

    public const string _sceneBasePath = "Scene/";
    public const string _TableBasePath = "Table/";
    public const string _movieBasePath = "Movie/";
    public const string _soundBasePath = "Sound/";

    public const string _assetBundleExtension = ".unity3d";
    public const string _aniClipExtension = ".anim";
    public const string _aniControllerExtension = ".controller";
    public const string _prefabExtension = ".prefab";
    public const string _materialExtension = ".mat";
    public const string _bytesFileExtension = ".bytes";
    public const string _sceneFileExtension = ".unity";
    public const string _soundFileExtension = ".wav";
    public const string _musicFileExtension = ".ogg";
    public const string _tgaFileExtension = ".tga";
    public const string _pngFileExtension = ".png";
    public const string _movieFileExtension = ".mp4";
    public const string _scriptFileExtension = ".cs";

    public static string[] _excludeAssetBundleExtension = 
    {
        _scriptFileExtension
    };

    public static string GetPlatformName()
    {
#if UNITY_EDITOR
        return GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
        return GetPlatformFolderForAssetBundles(Application.platform);
#endif
    }

#if UNITY_EDITOR
    private static string GetPlatformFolderForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            default:
                Debug.LogError(target + " does not support build target.");
                return null;
        }
    }
#endif

    private static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            default:
                Debug.LogError(platform + " does not support platform");
                return null;
        }
    }
}
