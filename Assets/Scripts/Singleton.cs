using UnityEngine;

public class Singleton<T> where T : class, new()
{
    protected static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                Debug.LogError("More than Single instance " + typeof(T));
                return instance;
            }

            instance = new T();
            return instance;
        }
    }

    public static void DestoryInstance()
    {
        instance = null;
    }
}

public class SingletonGameObject<T> : MonoBehaviour where T : class
{
    public static string defaultName = "Singleton_";

    protected static T instance = null;
    private static GameObject obj = null;

    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                Debug.LogError("More than Single instance " + typeof(T));
                return instance;
            }

            instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                string name = string.Format("{0}{1}", defaultName, typeof(T));
                obj = new GameObject(name);
                DontDestroyOnLoad(obj);

                instance = obj.AddComponent(typeof(T)) as T;
            }

            return instance;
        }
    }

    public static void DestoryInstance()
    {
        if (null != obj)
        {
            GameObject.Destroy(obj);
            obj = null;
        }

        instance = null;
    }
}