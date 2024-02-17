using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StringNotch : MonoBehaviour
{
    public Action OnStringReleased;
    public Action OnArrowSet;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private Transform _rightBowNotch;

    [SerializeField]
    private Transform _leftBowNotch;

    [SerializeField]
    private float _arrowLength;

    [SerializeField]
    private InteractionLayerMask _interactionLayerMask;

    private Arrow _activeArrow = null;
    private GrabManager _grabManager;
    private XRGrabInteractable _interactable;

    private void Start()
    {
        _grabManager = GrabManager.Instance;
        _interactable = GetComponent<XRGrabInteractable>();
    }

    public void UpdateLimitedNotchPosition(Vector3 limitedPosition)
    {
        if (_activeArrow == null)
            return;

        _activeArrow.transform.position = limitedPosition;

        bool isStringInRightHand = _grabManager.IsTagInRightHand(TagDirectory.Notch);
        var activeBowNotch = isStringInRightHand ? _rightBowNotch : _leftBowNotch;

        var direction = (activeBowNotch.position - limitedPosition).normalized;
        var lookAtPosition = limitedPosition + direction * _arrowLength * 2.0f;
        _activeArrow.gameObject.transform.LookAt(lookAtPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != Layers.Arrow || other.transform.parent == null)
            return;

        if (!_grabManager.IsHoldingLayer(Layers.Bow) || !_grabManager.IsHoldingLayer(Layers.Arrow))
            return;

        if (!_grabManager.IsGameobjectHeld(other.transform.parent.gameObject))
            return;

        if (other.gameObject.tag == TagDirectory.Notch)
        {
            _activeArrow = other.transform.parent.gameObject.GetComponent<Arrow>();
            _activeArrow.SetFollowHandRotation(false);

            _interactable.interactionLayers = _interactionLayerMask;

            var handWithArrow = _grabManager.IsLayerInLeftHand(Layers.Arrow)
                ? Hand.Left
                : Hand.Right;

            _grabManager.SetItemInHand(gameObject, handWithArrow);
            OnArrowSet?.Invoke();
        }
    }

    public void Release()
    {
        if (_activeArrow == null) // Arrow was unstrung
            return;

        OnStringReleased?.Invoke();
        _interactable.interactionLayers = 0;
    }

    public void UnstringArrow()
    {
        bool isStringInRightHand = _grabManager.IsTagInRightHand(TagDirectory.Notch);
        Hand activeHand = isStringInRightHand ? Hand.Right : Hand.Left;

        _activeArrow.SetFollowHandRotation(true);
        _grabManager.SetItemInHand(_activeArrow.gameObject, activeHand);
        _interactable.interactionLayers = 0;
        _activeArrow = null;
    }

    public void FireArrow(float power, Vector3 direction)
    {
        _activeArrow.FireArrow(power, direction);
        _activeArrow = null;
    }
}
