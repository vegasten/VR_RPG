using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryUI;

    [SerializeField]
    private GameObject _inventoryItemPrefab;

    [SerializeField]
    private Transform _itemsContainer;

    private void Start()
    {
        EnableUI(false);        
    }

    public void EnableUI(bool enable)
    {
        _inventoryUI.SetActive(enable);
    }

    public void UpdateInventoryUI(List<InventoryItem> items)
    {
        RemoveAllItems();
        AddItems(items);
    }

    private void AddItems(List<InventoryItem> items)
    {
        foreach (var item in items) {

            var inventoryItemPresenter = Instantiate(_inventoryItemPrefab, _itemsContainer).GetComponent<InventoryItemPresenter>();
            inventoryItemPresenter.SetCount(item.Stack);
            inventoryItemPresenter.SetName(item.Item.DisplayName);
        }
    }

    private void RemoveAllItems()
    {
        int numberOfChildren = _itemsContainer.childCount;

        for (int i = numberOfChildren -1; i >= 0; i--)
        {
            Destroy(_itemsContainer.GetChild(i).gameObject);
        }
    }
}
