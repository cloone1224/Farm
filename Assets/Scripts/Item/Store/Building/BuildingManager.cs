using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    #region Singleton
    private static BuildingManager instance = null;
    public static BuildingManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than BuildingManager instance.");
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
