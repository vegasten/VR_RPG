using System.Collections;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _itemDropParticleEffect;

    [SerializeField]
    private float _timeBeforeDespawning = 30f;

    [SerializeField]
    private float _particleEffectHeight = 1.5f;

    public string DisplayName { get; private set; } = "Placeholder item name";

    private Rigidbody _rigidbody;
    private bool _particleEffectIsActive = false;

    private float _timeWithoutMovement = 0.0f;
    private ItemDropManager _itemDropManager;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(DestroyAfterTime());

        _itemDropManager = ItemDropManager.Instance;
    }

    private void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TagDirectory.LeftDirectInteractor)
        {
            _itemDropManager.RegisterItemDropInRange(this, Hand.Left);
        }

        if (other.gameObject.tag == TagDirectory.RightDirectInteractor)
        {
            _itemDropManager.RegisterItemDropInRange(this, Hand.Right);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Entered: {other.gameObject}");

        if (other.gameObject.tag == TagDirectory.LeftDirectInteractor)
        {
            _itemDropManager.RemoveItemDropInRange(this, Hand.Left);
        }

        if (other.gameObject.tag == TagDirectory.RightDirectInteractor)
        {
            _itemDropManager.RemoveItemDropInRange(this, Hand.Right);
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
