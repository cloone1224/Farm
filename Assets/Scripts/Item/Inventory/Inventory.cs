using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton
    private static Inventory instance = null;
    public static Inventory Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of Inventory found!");
            DestroyImmediate(this);
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    [SerializeField]
    private int space = 20;
    public int Space { get { return space; } private set { } }

    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Do not enough room.");
                return false;
            }

            items.Add(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
