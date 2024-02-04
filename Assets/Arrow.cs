using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Action OnRelease;

    [SerializeField] private Transform _arrowNotch;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    public void Release()
    {
        OnRelease?.Invoke();
    }

    public Vector3 GetArrowNotchPosition()
    {
        return _arrowNotch.position;
    }

    public void FireArrow(float power, Vector3 direction)
    {
        EnablePhysics();
        Vector3 force = power * direction;
        _rigidBody.AddForce(force, ForceMode.Impulse);
    }

    private void EnablePhysics()
    {
        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = true;
    }
}
