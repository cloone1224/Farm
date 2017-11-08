using UnityEngine;
using System.Collections.Generic;

public class HttpManager : MonoBehaviour {

    #region Singleton
    private static HttpManager instance = null;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of HttpManager found!");
            DestroyImmediate(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public interface IObserver
    {
        void HttpResponseHandler();
    }

    #region Regist & Unregist observer
    public void RegisterObserver(IObserver newObserver)
    {
        if (observers.Contains(newObserver))
            return;
        observers.Add(newObserver);
    }

    public void UnregisterObserver(IObserver removeObserver)
    {
        if (!observers.Contains(removeObserver))
            return;
        observers.Remove(removeObserver);
    }
    #endregion

    [SerializeField]
    public string defaultUrl = ""; // Get urls

    Dictionary<string, string> httpUrls = new Dictionary<string, string>();
    List<IObserver> observers = new List<IObserver>();

    public void SetUrl(string name, string url)
    {
        if (httpUrls.ContainsKey(name))
        {
            httpUrls.Remove(name);
        }

        httpUrls.Add(name, url);
    }


}
