using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != Layers.Arrow)
            return;

        if (other.gameObject.tag != TagDirectory.Arrow)
            return;

        var damageData = other.GetComponent<Arrow>().GetArrowDamageData();
        var damage = damageData.BaseDamage * damageData.Speed;

        Debug.Log($"Took damage: {damage}");
    }
}
