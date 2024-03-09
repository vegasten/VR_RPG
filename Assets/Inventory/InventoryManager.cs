using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> _inventoryItems;
    private ItemDatabase _itemDatabase;

    private void Start()
    {
        _itemDatabase = ItemDatabase.Instance;
        _inventoryItems = new List<InventoryItem>();
    }

    public void AddItemFromItemId(uint itemId)
    {
        AddItem(_itemDatabase.GetItem(itemId));
    }

    public void AddItem(ItemData itemToAdd, int numberOfItem = 1)
    {
        if (!itemToAdd.Stackable)
        {
            _inventoryItems.Add(new InventoryItem { Stack = 1, Item = itemToAdd });
            return;
        }

        var index = _inventoryItems.FindIndex(x => x.Item.ItemId == itemToAdd.ItemId);

        if (index != -1)
        {
            _inventoryItems[index].Stack += numberOfItem;
        }
        else
        {
            _inventoryItems.Add(new InventoryItem { Stack = 1, Item = itemToAdd });
        }
    }
}

public class InventoryItem
{
    public int Stack;
    public ItemData Item;
}
