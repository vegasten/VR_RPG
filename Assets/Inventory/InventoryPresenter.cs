using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryUI;

    private void Start()
    {
        EnableUI(false);
    }

    public void EnableUI(bool enable)
    {
        _inventoryUI.SetActive(enable);
    }
}
