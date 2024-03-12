using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private ItemDropManager _itemDropManager;

    [SerializeField]
    private InventoryManager _inventoryManager;

    private XRIDefaultInputActions _input = null;

    private void Awake()
    {
        _input = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.XRILeftHandInteraction.Activate.performed += OnLeftActivated;
        _input.XRIRightHandInteraction.Activate.performed += OnRightActivated;
        _input.XRILeftHandInteraction.X.performed += onX;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.XRILeftHandInteraction.Activate.performed -= OnLeftActivated;
        _input.XRIRightHandInteraction.Activate.performed -= OnRightActivated;
        _input.XRILeftHandInteraction.X.performed -= onX;
    }

    private void OnLeftActivated(InputAction.CallbackContext context)
    {
        var closestItemLeft = _itemDropManager.GetClosestItemLeftHand();
        _inventoryManager.AddItemFromItemId(closestItemLeft.ItemId);
        _itemDropManager.RemoveItemDropInRange(closestItemLeft, Hand.Left);
        _itemDropManager.RemoveItemDropInRange(closestItemLeft, Hand.Right);
        closestItemLeft.DestroyAfterPickup();
    }

    private void OnRightActivated(InputAction.CallbackContext context)
    {
        var closestItemRight = _itemDropManager.GetClosestItemRightHand();
        _inventoryManager.AddItemFromItemId(closestItemRight.ItemId);
        _itemDropManager.RemoveItemDropInRange(closestItemRight, Hand.Left);
        _itemDropManager.RemoveItemDropInRange(closestItemRight, Hand.Right);
        closestItemRight.DestroyAfterPickup();
    }

    private void onX(InputAction.CallbackContext context)
    {
        _inventoryManager.ToggleInventoryUI();
    }
}
