using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _killParticleEffect;

    [SerializeField]
    private LootManager _lootManager;

    [SerializeField]
    private uint _enemyId;

    MeshRenderer _meshRenderer;
    Material _material;

    bool _isKilled = false;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isKilled) return;

        if (other.gameObject.layer != Layers.Arrow)
            return;

        if (other.gameObject.tag != TagDirectory.Arrow)
            return;

        var damageData = other.GetComponent<Arrow>().GetArrowDamageData();
        var damage = damageData.BaseDamage * damageData.Speed;

        Debug.Log($"Took damage: {damage}");
        Kill();
    }

    private void Kill()
    {
        _isKilled = true;
        StartCoroutine(KillCoroutine());
        _lootManager.CreateDrop(_enemyId, transform.position);
    }

    private IEnumerator KillCoroutine()
    {

        float dissolveFactor = 1f;
        float dissolveTime = 3f;

        DOTween.To(() => dissolveFactor, x => dissolveFactor = x, 0f, dissolveTime)
            .OnUpdate(() => _material.SetFloat("_DissolveFactor", dissolveFactor));
        _killParticleEffect.Play();

        yield return new WaitForSeconds(dissolveTime);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}
