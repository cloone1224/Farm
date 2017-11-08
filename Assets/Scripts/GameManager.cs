using UnityEngine;
using System;

public class GameManager : SingletonGameObject<GameManager> {

    [Serializable]
    public struct GameVersion
    {
        public int major;
        public int minor;
        public int patch;

        public GameVersion(int major, int minor, int patch)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }
    }

    [Serializable]
    public class GameConfigInfo
    {
        [SerializeField]
        private string productName = "UnknownHeroes";
        [SerializeField]
        private string bundleName = "com.nexon.unknownheroes";
        [SerializeField]
        private GameVersion version = new GameVersion(1, 0, 0);
        [SerializeField]
        private int versionCode = 0;
        [SerializeField]
        private string androidKeyaliasName = "unknownheroes";
        [SerializeField]
        private string androidKeyaliasPass = "unknownheroes123!@#";
        [SerializeField]
        private string androidKeystoreName = "unknownheroes.jks";
        [SerializeField]
        private string iosTargetVersion = "8.0"; // major.minor
        [SerializeField]
        private bool iosUseBuildHistory = true;

        public string ProductName { get { return productName; } }
        public string BundleName { get { return bundleName; } }

        public int VersionCode { get { return versionCode; } }
        public string KeyaliasName { get { return androidKeyaliasName; } }
        public string KeyaliasPass { get { return androidKeyaliasPass; } }
        public string KeystoreName { get { return androidKeystoreName; } }

        public string IOSTargetVersion { get { return iosTargetVersion; } }
        public bool IOSUseBuildHistory { get { return iosUseBuildHistory; } }

        public string GetVersionName()
        {
            return version.major + "." + version.minor + "." + version.patch;
        }

        public string GetVersionCode()
        {
            return versionCode.ToString();
        }
    }

    [Header("Game Build Configs:")]
    [SerializeField]
    private GameConfigInfo gameConfig = new GameConfigInfo();

    [Header("Game Application Settings:")]
    [SerializeField]
    private int targetFrameRate = 30;

    public GameConfigInfo GameConfig
    {
        get { return gameConfig; } private set { }
    }

    // Use this for initialization
    void Start() {

        DontDestroyOnLoad(gameObject);

        InitializeApplication();

        Initialize();
    }

    void InitializeApplication()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        Application.targetFrameRate = targetFrameRate;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Initialize()
    {

    }

    void Update()
    {

    }

    void LateUpdate()
    {

    }

    void FixedUpdate()
    {

    }
}
