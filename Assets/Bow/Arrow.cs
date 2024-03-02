using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : MonoBehaviour
{
    public Action OnRelease;
    public Action<GameObject> ReleaseToPool;

    [SerializeField]
    private Transform _arrowNotch;

    [SerializeField]
    private float _baseDamage = 3f;

    private Rigidbody _rigidBody;
    private bool _inFlight = false;
    private XRGrabInteractable _grabInteractable;
    private float _secondsBeforeReleasingToPool = 10f;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void FixedUpdate()
    {
        if (_inFlight)
        {
            if (_rigidBody.velocity != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(_rigidBody.velocity, transform.up);
                transform.rotation = newRotation;
            }
        }
    }

    public void FireArrow(float power, Vector3 direction)
    {
        EnablePhysics();
        _inFlight = true;
        Vector3 force = power * direction;
        _rigidBody.AddForce(force, ForceMode.Impulse);
        StartCoroutine(ReleaseToPoolCounter());
    }

    public ArrowDamageData GetArrowDamageData()
    {
        var speed = GetComponent<Rigidbody>().velocity.magnitude;
        return new ArrowDamageData(_baseDamage, speed);
    }

    private void EnablePhysics()
    {
        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = true;
    }

    public void DisablePhysics()
    {
        _rigidBody.isKinematic = true;
        _rigidBody.useGravity = false;
    }

    public void SetFollowHandRotation(bool follow)
    {
        _grabInteractable.trackRotation = follow;
    }

    private IEnumerator ReleaseToPoolCounter()
    {
        yield return new WaitForSeconds(_secondsBeforeReleasingToPool);
        ReleaseToPool?.Invoke(gameObject);
    }
}
