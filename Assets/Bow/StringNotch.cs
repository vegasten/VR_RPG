using System;
using UnityEngine;

public class StringNotch : MonoBehaviour
{
    public Action OnStringReleased;
    public Action OnStringNotchedMoved;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private Transform _rightBowNotch;

    [SerializeField]
    private Transform _leftBowNotch;

    [SerializeField]
    private float _arrowLength;

    private Arrow _activeArrow = null;
    private GrabManager _grabManager;

    private void Start()
    {
        _grabManager = GrabManager.Instance;
    }

    private void Update()
    {
        if (_activeArrow == null)
            return;

        transform.position = _activeArrow.GetArrowNotchPosition();
        OnStringNotchedMoved?.Invoke();

        bool isBowInRightHand = _grabManager.IsLayerInRightHand(Layers.Bow);
        var activeBowNotch = isBowInRightHand ? _leftBowNotch : _rightBowNotch;

        var direction = (activeBowNotch.position - transform.position).normalized;
        var lookAtPosition = transform.position + direction * _arrowLength * 2.0f; // TODO limit the pull distance of the notch, should be enough?
        _activeArrow.gameObject.transform.LookAt(lookAtPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != Layers.Arrow)
            return;

        if (!_grabManager.IsHoldingLayer(Layers.Bow) || !_grabManager.IsHoldingLayer(Layers.Arrow))
            return;

        if (!_grabManager.IsGameobjectHeld(other.transform.parent.gameObject))
            return;

        if (other.gameObject.tag == TagDirectory.Notch)
        {
            _activeArrow = other.transform.parent.gameObject.GetComponent<Arrow>();
            _activeArrow.OnRelease += OnArrowReleased;
            _activeArrow.TurnOffFollowHandRotation();
        }
    }

    private void OnArrowReleased()
    {
        OnStringReleased?.Invoke();
    }

    public void FireArrow(float power, Vector3 direction)
    {
        _activeArrow.FireArrow(power, direction);
        _activeArrow.OnRelease -= OnArrowReleased;
        _activeArrow = null;
    }
}
