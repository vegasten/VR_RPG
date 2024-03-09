using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    [SerializeField]
    private int _arrowsToKeep = 5;

    [SerializeField]
    private GameObject _arrowPrefab;

    private Queue<GameObject> _arrows = new Queue<GameObject>();

    public GameObject TakeArrow()
    {
        if (!_arrows.Any())
        {
            return Instantiate(_arrowPrefab);
        }

        var arrow = _arrows.Dequeue();
        arrow.SetActive(true);
        return arrow;
    }

    public void ReturnArrow(GameObject arrowGameObject)
    {
        if (_arrows.Count >= _arrowsToKeep)
        {
            Destroy(arrowGameObject);
            CleanUpPool();
            return;
        }

        var arrow = arrowGameObject.GetComponent<Arrow>();
        arrow.SetFollowHandRotation(true);
        arrow.DisablePhysics();

        arrowGameObject.SetActive(false);
        _arrows.Enqueue(arrowGameObject);
    }

    private void CleanUpPool()
    {
        if (_arrows.Count > _arrowsToKeep)
        {
            _arrows.Dequeue();
            CleanUpPool();
        }
    }
}
