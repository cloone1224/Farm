using System;
using UnityEngine;

public class Item : ScriptableObject {

    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void Use()
    {
        
    }
}
