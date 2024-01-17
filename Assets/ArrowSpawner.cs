using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private GameObject _notch;

    private XRGrabInteractable _bow;
    private bool _arrowNotched = false;
    private GameObject _currentArrow;

    void Start()
    {
        _bow = GetComponent<XRGrabInteractable>();
        StringPuller.OnStringReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        StringPuller.OnStringReleased -= NotchEmpty;
    }

    void Update()
    {
        if (_bow.isSelected && !_arrowNotched)
        {
            StartCoroutine(DelayedSpawn());
        } 
        if (!_bow.isSelected) {
            Destroy(_currentArrow);
        }
    }

    private void NotchEmpty(float pullAmount)
    {
        _arrowNotched = false;
    }

    IEnumerator DelayedSpawn()
    {
        _arrowNotched = true;
        yield return new WaitForSeconds(1f);
        _currentArrow = Instantiate(_arrow, _notch.transform);
    }
}
