using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Action OnRelease;

    [SerializeField] private Transform _arrowNotch;

    public void Release()
    {
        OnRelease?.Invoke();
    }

    public Vector3 GetArrowNotchPosition()
    {
        return _arrowNotch.position;
    }
}
