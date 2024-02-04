using System;
using UnityEngine;

public class StringNotch : MonoBehaviour
{
    public Action OnStringReleased;
    public Action OnStringNotchedMoved;

    [SerializeField] private LayerMask _layerMask;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_grabManager.IsHoldingLayer(Layers.Bow) || !_grabManager.IsHoldingLayer(Layers.Arrow))
            return;

        if (other.gameObject.tag == TagDirectory.Notch)
        {
            Debug.Log("Active arrow set!");

            _activeArrow = other.transform.parent.gameObject.GetComponent<Arrow>();
            _activeArrow.OnRelease += OnArrowReleased;
        }

    }

    private void OnArrowReleased()
    {
        Debug.Log("Arrow released");

        OnStringReleased?.Invoke();
        _activeArrow.OnRelease -= OnArrowReleased;
        _activeArrow = null;
    }
}
