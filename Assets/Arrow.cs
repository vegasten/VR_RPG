using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _tip;

    private Rigidbody _rigidBody;
    private bool _inAir = false;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        StringPuller.OnStringReleased += Release;

        Stop();
    }

    private void OnDestroy()
    {
        StringPuller.OnStringReleased -= Release;
    }

    private void Release(float value)
    {
        StringPuller.OnStringReleased -= Release;
        gameObject.transform.parent = null;
        _inAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * value * _speed;
        _rigidBody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            var newRotation = Quaternion.LookRotation(_rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer != 8)
        {
            if (collision.transform.TryGetComponent(out Rigidbody body))
            {
                _rigidBody.interpolation = RigidbodyInterpolation.None;
                transform.parent = collision.transform;
                body.AddForce(_rigidBody.velocity, ForceMode.Impulse);
            }
            Stop();
        }
    }

    private void Stop()
    {
        _inAir = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        _rigidBody.useGravity = usePhysics;
        _rigidBody.isKinematic = !usePhysics;
    }
}
