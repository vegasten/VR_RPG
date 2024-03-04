using System;
using System.Collections;
using System.Threading.Tasks;
using System.Xml.Schema;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _itemDropParticleEffect;

    [SerializeField]
    private float _timeBeforeDespawning = 30f;

    [SerializeField]
    private float _particleEffectHeight = 1.5f;

    private Rigidbody _rigidbody;
    private bool _particleEffectIsActive = false;

    private float _timeWithoutMovement = 0.0f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(DestroyAfterTime());
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("FORCE");
        _rigidbody.AddForce(Vector3.left * 100f);
    }

    private void Update()
    {
        Debug.Log(_timeWithoutMovement);

        var currentlyMoving = IsMoving();
        if (!currentlyMoving)
        {
            _timeWithoutMovement += Time.deltaTime;
        }
        else
        {
            _timeWithoutMovement = 0;
        }

        bool isMoving = _timeWithoutMovement < 1.5f; // Time threshold before assuming to be still

        if (!isMoving && !_particleEffectIsActive)
        {
            StartParticleEffect();
            _particleEffectIsActive = true;
            return;
        }

        if (isMoving)
        {
            StopParticleEffect();
            _particleEffectIsActive = false;
        }
    }

    private bool IsMoving()
    {
        return _rigidbody.velocity.sqrMagnitude >= 0.001f;
    }

    private void StartParticleEffect()
    {
        UpdateParticleEffectPosition();
        _itemDropParticleEffect.Play();
    }

    private void UpdateParticleEffectPosition()
    {
        _itemDropParticleEffect.transform.position =
            transform.position + Vector3.up * _particleEffectHeight;
    }

    private void StopParticleEffect()
    {
        _itemDropParticleEffect.Stop();
        _itemDropParticleEffect.Clear();
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(_timeBeforeDespawning);
        Destroy(gameObject);
    }
}
