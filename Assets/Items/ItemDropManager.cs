using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    [SerializeField]
    private HandDisplayPresenter _leftHandDisplay;

    [SerializeField]
    private HandDisplayPresenter _rightHandDisplay;

    [SerializeField]
    private Transform _leftHand;

    [SerializeField]
    private Transform _rightHand;

    [SerializeField]
    private InventoryManager inventoryManager;

    public static ItemDropManager Instance;

    List<ItemDrop> _itemDropsInRangeRightHand;
    List<ItemDrop> _itemDropsInRangeLeftHand;

    private ItemDrop _activeItemLeft = null;
    private ItemDrop _activeItemRight = null;

    private ItemDatabase _itemDatabase;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _itemDropsInRangeRightHand = new List<ItemDrop>();
        _itemDropsInRangeLeftHand = new List<ItemDrop>();
        _itemDatabase = ItemDatabase.Instance;
    }

    private void Update()
    {
        var closestItemLeftHand = GetClosestItemLeftHand();
        var closestItemRightHand = GetClosestItemRightHand();

        if (closestItemLeftHand != null)
        {
            var itemData = _itemDatabase.GetItem(closestItemLeftHand.ItemId);

            _leftHandDisplay.Enable(true);
            _leftHandDisplay.SetText(itemData.DisplayName);
            _activeItemLeft = closestItemLeftHand;
        }
        else
        {
            _leftHandDisplay.Enable(false);
            _activeItemLeft = null;
        }

        if (closestItemRightHand != null)
        {
            var itemData = _itemDatabase.GetItem(closestItemRightHand.ItemId);

            _rightHandDisplay.Enable(true);
            _rightHandDisplay.SetText(itemData.DisplayName);
            _activeItemRight = closestItemRightHand;
        }
        else
        {
            _rightHandDisplay.Enable(false);
            _activeItemRight = null;
        }
    }

    public void RegisterItemDropInRange(ItemDrop itemDrop, Hand hand)
    {
        if (hand == Hand.Left)
        {
            _itemDropsInRangeLeftHand.Add(itemDrop);
        }
        else if (hand == Hand.Right)
        {
            _itemDropsInRangeRightHand.Add(itemDrop);
        }
    }

    public void RemoveItemDropInRange(ItemDrop itemDrop, Hand hand)
    {
        if (hand == Hand.Left)
        {
            _itemDropsInRangeLeftHand.Remove(itemDrop);
        }
        else if (hand == Hand.Right)
        {
            _itemDropsInRangeRightHand.Remove(itemDrop);
        }
    }

    public ItemDrop GetClosestItemLeftHand()
    {
        return _itemDropsInRangeLeftHand
            .OrderBy(itemDrop => Vector3.Distance(itemDrop.transform.position, _leftHand.position))
            .FirstOrDefault();
    }

    public ItemDrop GetClosestItemRightHand()
    {
        return _itemDropsInRangeRightHand
            .OrderBy(itemDrop => Vector3.Distance(itemDrop.transform.position, _rightHand.position))
            .FirstOrDefault();
    }
}
