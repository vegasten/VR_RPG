using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowCombatManager : MonoBehaviour
{
    [SerializeField]
    private ArrowPool _arrowPool;

    [SerializeField]
    private XRInteractionManager _interactionManager;

    [SerializeField]
    private BehindTheHeadGrabZone _grabZone;

    private GrabManager _grabManager;

    private void Start()
    {
        _grabManager = GrabManager.Instance;

        _grabZone.Selected += SpawnArrowInHand;
    }

    private void OnDestroy()
    {
        _grabZone.Selected -= SpawnArrowInHand;
    }

    private void SpawnArrowInHand()
    {
        if (!HoldsBowAndGrabZone())
        {
            return;
        }

        var freehand = _grabManager.IsLayerInLeftHand(Layers.Bow) ? Hand.Right : Hand.Left;

        var arrowGameObject = _arrowPool.TakeArrow();
        arrowGameObject.GetComponent<Arrow>().ReleaseToPool += ReleaseArrowToPool;
        _grabManager.SetItemInHand(arrowGameObject, freehand);
    }

    private void ReleaseArrowToPool(GameObject arrow)
    {
        arrow.GetComponent<Arrow>().ReleaseToPool -= ReleaseArrowToPool;
        _arrowPool.ReturnArrow(arrow);
    }

    private bool HoldsBowAndGrabZone()
    {
        if (
            _grabManager.IsLayerInLeftHand(Layers.Bow)
            && _grabManager.IsLayerInRightHand(Layers.GrabTrigger)
        )
        {
            return true;
        }
        else if (
            _grabManager.IsLayerInRightHand(Layers.Bow)
            && _grabManager.IsLayerInLeftHand(Layers.GrabTrigger)
        )
        {
            return true;
        }
        else
            return false;
    }
}
