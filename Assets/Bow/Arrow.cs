using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : MonoBehaviour
{
    public Action OnRelease;

    [SerializeField]
    private Transform _arrowNotch;

    private Rigidbody _rigidBody;
    private bool _inFlight = false;
    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void FixedUpdate()
    {
        if (_inFlight)
        {
            Quaternion newRotation = Quaternion.LookRotation(_rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
        }
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
        _inFlight = true;
        Vector3 force = power * direction;
        _rigidBody.AddForce(force, ForceMode.Impulse);
    }

    private void EnablePhysics()
    {
        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = true;
    }

    public void TurnOffFollowHandRotation()
    {
        _grabInteractable.trackRotation = false;
    }
}
