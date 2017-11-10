using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
// Unity3D 메뉴 Build에 Build 메뉴를 추가하여 현재 설정되어 있는 플랫폼에 맞는 빌드를 수행해준다.
public class BuildScript {

    private static readonly string[] scenes;
    private static readonly string targetDir = "Build";
    
    private static string projectPath;
    private static string buildPath;

    static BuildScript()
    {
        GameManager.GameConfigInfo gameConfig = GameManager.Instance.GameConfig;

        PlayerSettings.productName = gameConfig.ProductName;
        //PlayerSettings.bundleIdentifier = gameConfig.BundleName;
        //PlayerSettings.applicationIdentifier = gameConfig.BundleName;
        PlayerSettings.bundleVersion = gameConfig.GetVersionName();

        projectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
        buildPath = string.Format("{0}/{1}", projectPath, targetDir);

        scenes = FindEnabledEditorScenes();
    }

    [MenuItem("Build/Build")]
    static void Build()
    {
        // 지정된 플랫폼에 맞는 빌드 수행
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.StandaloneWindows: BuildWindows32(); break;
            case BuildTarget.StandaloneWindows64: BuildWindows64(); break;
            case BuildTarget.Android: BuildAndroid(); break;
            case BuildTarget.iOS: BuildiOS(); break;
            case BuildTarget.StandaloneOSXUniversal: BuildOSX(); break;
            default:
                {
                    Debug.LogWarning(EditorUserBuildSettings.activeBuildTarget + " is not support build target.");
                    break;
                }
        }
    }

    static void BuildWindows(BuildTarget buildTarget)
    {
        string outputPath = string.Format("{0}/windows32", buildPath);

        CreateDirectory(outputPath);

        string executeName = string.Format("{0}.exe", PlayerSettings.productName);
        string targetDir = string.Format("{0}/{1}", outputPath, executeName);

        BuildOptions buildOptions = GetBuildOptions();
        bool success = ProcessBuild(scenes, targetDir, buildTarget, buildOptions);

        if (success)
        {
            // 실행
            RunBuild(targetDir);
        }
    }

    static void BuildWindows32()
    {
        BuildWindows(BuildTarget.StandaloneWindows);
    }

    static void BuildWindows64()
    {
        BuildWindows(BuildTarget.StandaloneWindows64);
    }

    static void BuildAndroid()
    {
        string outputPath = string.Format("{0}/android", buildPath);

        CreateDirectory(outputPath);

        string executeName = string.Format("{0}.apk", PlayerSettings.productName);
        string targetDir = string.Format("{0}/{1}", outputPath, executeName);

        GameManager.GameConfigInfo gameConfig = GameManager.Instance.GameConfig;

        PlayerSettings.Android.bundleVersionCode = gameConfig.VersionCode;
        
        string keystorePath = string.Format("{0}/helper/android_keystore/{1}", buildPath, gameConfig.KeystoreName);

        PlayerSettings.Android.keystoreName = keystorePath;
        PlayerSettings.Android.keystorePass = gameConfig.KeyaliasPass;
        PlayerSettings.Android.keyaliasName = gameConfig.KeyaliasName;
        PlayerSettings.Android.keyaliasPass = gameConfig.KeyaliasPass;

        BuildOptions buildOptions = GetBuildOptions();
        bool success = ProcessBuild(scenes, targetDir, BuildTarget.Android, buildOptions);

        if (success)
        {
            // 단말기 연결시 설치 및 실행
        }
    }

    static void BuildiOS()
    {
        string outputPath = string.Format("{0}/ios", buildPath);

        CreateDirectory(outputPath);

        GameManager.GameConfigInfo gameConfig = GameManager.Instance.GameConfig;

        PlayerSettings.iOS.buildNumber = gameConfig.GetVersionCode();
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        //PlayerSettings.iOS.targetOSVersion = iOSTargetOSVersion.iOS_8_0;
        PlayerSettings.iOS.targetOSVersionString = gameConfig.IOSTargetVersion;
        PlayerSettings.statusBarHidden = true;

        BuildOptions buildOptions = GetBuildOptions();

        // 최초 빌드시에는 제거한다.(이 옵션의 의미는 Unity에서 프로젝트를 iOS버전으로 빌드할때 현재 이미 빌드된 프로젝트 파일들이 있을 경우 Replace하지 말고 Merge하라는 의미)
        // XCode 프로젝트를 생성하는 첫번째 빌드시에는 제외시켜야 한다.
        if (gameConfig.IOSUseBuildHistory && PlayerPrefs.GetInt("BuildHistory") == 1)
        {
            buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;
        }

        bool success = ProcessBuild(scenes, outputPath, BuildTarget.iOS, buildOptions);

        if (success)
        {
            // Nothing Xcode project
            PlayerPrefs.SetInt("BuildHistory", 1);
            PlayerPrefs.Save();       
        }
    }

    static void BuildOSX()
    {
        string outputPath = string.Format("{0}/osx", buildPath);

        CreateDirectory(outputPath);

        string executeName = string.Format("{0}.app", PlayerSettings.productName);
        string targetDir = string.Format("{0}/{1}", outputPath, executeName);

        BuildOptions buildOptions = GetBuildOptions();

        bool success = ProcessBuild(scenes, targetDir, BuildTarget.StandaloneOSXUniversal, buildOptions);

        if (success)
        {
            // 실행
        }
    }

    // 실제 빌드 함수
    private static bool ProcessBuild(string[] scenes, string targetDir, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        // 빌드 메뉴를 플랫폼별로 생성시에만 필요.
        // 플랫폼별로 프로젝트를 생성하는게 관리 소홀
        if (buildTarget != EditorUserBuildSettings.activeBuildTarget)
        {
            Debug.LogError(buildTarget + " does not matched with " + EditorUserBuildSettings.activeBuildTarget);
            // 필요시 EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget) 를 통해서 변경 가능
            return false;
        }

        bool buildSuccess = true;

        string message = BuildPipeline.BuildPlayer(scenes, targetDir, buildTarget, buildOptions);        
        if (!string.IsNullOrEmpty(message))
        {
            buildSuccess = false;
            throw new System.Exception(buildTarget + " processBuild failed.");
        }

        if (!buildSuccess)
            return false;

        Debug.Log(buildTarget + " processBuild succeed.");
        
        return AfterProcessBuild(targetDir, buildTarget);
    }

    // Unity3D 이후 추가 빌드가 필요할 경우 구현
    private static bool AfterProcessBuild(string targetDir, BuildTarget buildTarget)
    {
        return true;
    }

    //
    private static string[] FindEnabledEditorScenes()
    {
        List<string> editorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
                continue;

            editorScenes.Add(scene.path);
        }

        return editorScenes.ToArray();
    }

    private static void RunBuild(string executeName)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        
        process.StartInfo.FileName = executeName;
        process.Start();
    }

    private static bool CreateDirectory(string directory)
    {
        if (System.IO.Directory.Exists(directory))
            return true;

        System.IO.DirectoryInfo directoryInfo = System.IO.Directory.CreateDirectory(directory);
        return directoryInfo != null;
    }

    private static BuildOptions GetBuildOptions()
    {
        BuildOptions buildOptions = BuildOptions.None;

        if (EditorUserBuildSettings.symlinkLibraries) buildOptions |= BuildOptions.SymlinkLibraries;
        if (EditorUserBuildSettings.development) buildOptions |= BuildOptions.Development;
        if (EditorUserBuildSettings.connectProfiler) buildOptions |= BuildOptions.ConnectWithProfiler;
        if (EditorUserBuildSettings.allowDebugging) buildOptions |= BuildOptions.AllowDebugging;

        Debug.Log("BuildOptions: " + buildOptions);

        return buildOptions;
    }
}
#endif
