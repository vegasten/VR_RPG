using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _killParticleEffect;

    MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
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
        StartCoroutine(KillCoroutine());
    }

    private IEnumerator KillCoroutine()
    {
        _killParticleEffect.Play();
        yield return new WaitForSeconds(0.3f);
        _meshRenderer.enabled = false;
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
