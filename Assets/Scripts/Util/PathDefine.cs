using UnityEngine;

public static class PathDefine
{
    public static string _dataPath;
    public static string _streamingAssetsPath;

    /**
        참고: http://memocube.blogspot.kr/2014/04/blog-post.html
        
        Application.dataPath
            (O)Editor      project-dir/assets
            (O)Windows     execute-file/execute_data
            (X)Android     /data/app/bundlename-number.apk
            (X)iOS         /var/mobile/Applications/programid/appname.app/data

        Application.persistentDataPath
            (X)Editor      user-dir/appdata/locallow/company/productname
            (X)Windows     user-dir/appdata/locallow/company/productname
            (O)Android     /data/data/bundlename/files
            (O)iOS         /var/mobile/applications/programid/documents

        Application.streamingAssetsPath
            (O)Editor      project-dir/assets/streamingassets
            (O)Windows     execute-file/execute_data/streamingassets
            (O)Android     jar:file:///data/app/bundlename.apk!/assets
            (O)iOS         /var/mobile/applications/programid/appname.app/data/raw

    */

    static PathDefine()
    {
#if UNITY_EDITOR
        _dataPath = Application.dataPath.Replace("\\", "/"); // prodir/assets
#else
#if UNITY_ANDROID
        _dataPath = Application.persistentDataPath.Replace("\\", "/");
#elif UNITY_IPHONE
        _dataPath = Application.persistentDataPath.Replace("\\", "/");
#else // Windows
        _dataPath = Application.dataPath.Replace("\\", "/");
#endif
#endif
        _streamingAssetsPath = Application.streamingAssetsPath.Replace("\\", "/");
    }

    // filePath 은 폴더 경로를 가질수도 있다
    public static bool GetStreamingAssets(string filePath, out string fullPath)
    {
        if (filePath == null)
        {
            fullPath = null;
            return false;
        }

        filePath = filePath.Replace("\\", "/");
        if (filePath.StartsWith("/"))
            filePath = filePath.Substring(1);
        
        fullPath = string.Format("{0}/{1}", _streamingAssetsPath, filePath);
        return true;
    }

    // fileName 은 폴더 경로를 가질수도 있다
    public static bool GetPathForDocumentsFile(string filePath, out string fullPath)
    {
        if (filePath == null)
        {
            fullPath = null;
            return false;
        }

        filePath = filePath.Replace("\\", "/");
        if (filePath.StartsWith("/"))
            filePath = filePath.Substring(1);
        
        fullPath = string.Format("{0}/{1}", _dataPath, filePath);
        return true;
    }
}
