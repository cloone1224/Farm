using UnityEngine;

public class HudUI : MonoBehaviour {

    [SerializeField]
    private GameObject inventory;

    [SerializeField]
    private GameObject store;

    public void OnInventoryButton()
    {
        if (inventory == null)
            return;

        inventory.SetActive(!inventory.activeSelf);
    }

    public void OnStoreButton()
    {
        if (store == null)
            return;

        store.SetActive(!store.activeSelf);
    }
}
