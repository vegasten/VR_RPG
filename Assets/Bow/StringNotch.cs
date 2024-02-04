using System.Linq;
using UnityEngine;

public class StringNotch : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private string[] _tagsMask;

    private void OnTriggerEnter(Collider other)
    {
        if (_layerMask == (_layerMask | (1 << other.gameObject.layer)))
        {
            if (_tagsMask.Contains(other.gameObject.tag)) {
                Debug.Log("Hit string notch with arrow notch");
            }
        }
    }
}
