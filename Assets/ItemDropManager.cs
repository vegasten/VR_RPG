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

    public static ItemDropManager Instance;

    List<ItemDrop> _itemDropsInRangeRightHand;
    List<ItemDrop> _itemDropsInRangeLeftHand;

    private ItemDrop _activeItemLeft = null;
    private ItemDrop _activeItemRight = null;

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
    }

    private void Update()
    {
        var closestItemLeftHand = GetClosestItemLeftHand();
        var closestItemRightHand = GetClosestItemRightHand();

        if (closestItemLeftHand != null)
        {
            _leftHandDisplay.Enable(true);
            _leftHandDisplay.SetText(closestItemLeftHand.DisplayName);
            _activeItemLeft = closestItemLeftHand;
        }
        else
        {
            _leftHandDisplay.Enable(false);
            _activeItemLeft = null;
        }

        if (closestItemRightHand != null)
        {
            _rightHandDisplay.Enable(true);
            _rightHandDisplay.SetText(closestItemRightHand.DisplayName);
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

    private ItemDrop GetClosestItemLeftHand()
    {
        return _itemDropsInRangeLeftHand
            .OrderBy(itemDrop => Vector3.Distance(itemDrop.transform.position, _leftHand.position))
            .FirstOrDefault();
    }

    private ItemDrop GetClosestItemRightHand()
    {
        return _itemDropsInRangeRightHand
            .OrderBy(itemDrop => Vector3.Distance(itemDrop.transform.position, _rightHand.position))
            .FirstOrDefault();
    }
}
