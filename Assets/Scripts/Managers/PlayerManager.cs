using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    #region Singleton
    private static PlayerManager instance;
    public static PlayerManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of PlayerManager found!");
            DestroyImmediate(this);
            return;
        }

        instance = this;
    }
    #endregion

    public GameObject player;
}
