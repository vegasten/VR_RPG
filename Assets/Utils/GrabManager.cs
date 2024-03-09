using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GrabManager : MonoBehaviour
{
    [SerializeField]
    private XRInteractionManager _interactionManager;

    public static GrabManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public XRDirectInteractor _leftHandInteractor;
    public XRDirectInteractor _rightHandInteractor;

    public bool IsHoldingItem(Hand hand)
    {
        if (hand == Hand.Left)
            return _leftHandInteractor.hasSelection;
        else
            return _rightHandInteractor.hasSelection;
    }

    public GameObject GetItemInHand(Hand hand)
    {
        if (hand == Hand.Left)
        {
            return _leftHandInteractor.interactablesSelected
                .Select(x => x.transform.gameObject)
                .FirstOrDefault();
        }
        else
        {
            return _rightHandInteractor.interactablesSelected
                .Select(x => x.transform.gameObject)
                .FirstOrDefault();
        }
    }

    public bool IsHoldingTag(string tag)
    {
        var itemInRightHand = GetItemInHand(Hand.Right);
        var itemInLeftHand = GetItemInHand(Hand.Left);

        if (itemInRightHand != null && itemInRightHand.tag == tag)
            return true;
        else if (itemInLeftHand != null && itemInLeftHand.tag == tag)
            return true;
        return false;
    }

    public bool IsHoldingLayer(int layer)
    {
        var itemInRightHand = GetItemInHand(Hand.Right);
        var itemInLeftHand = GetItemInHand(Hand.Left);

        if (itemInRightHand != null && itemInRightHand.layer == layer)
            return true;
        if (itemInLeftHand != null && itemInLeftHand.layer == layer)
            return true;
        return false;
    }

    public bool IsLayerInRightHand(int layer)
    {
        var itemInRightHand = GetItemInHand(Hand.Right);

        if (itemInRightHand == null)
            return false;

        return itemInRightHand.layer == layer;
    }

    public bool IsLayerInLeftHand(int layer)
    {
        var itemInLeftHand = GetItemInHand(Hand.Left);

        if (itemInLeftHand == null)
            return false;

        return itemInLeftHand.layer == layer;
    }

    public bool IsTagInRightHand(string tag)
    {
        var itemInRightHand = GetItemInHand(Hand.Right);

        if (itemInRightHand == null)
            return false;

        return itemInRightHand.tag == tag;
    }

    public bool IsTagInLeftHand(string tag)
    {
        var itemInLeftHand = GetItemInHand(Hand.Left);

        if (itemInLeftHand == null)
            return false;

        return itemInLeftHand.tag == tag;
    }

    public bool IsGameobjectHeld(GameObject gameObject)
    {
        var itemInRight = GetItemInHand(Hand.Right);
        var itemInLeft = GetItemInHand(Hand.Left);

        return (
            ReferenceEquals(gameObject, itemInRight) || ReferenceEquals(gameObject, itemInLeft)
        );
    }

    public void SetItemInHand(GameObject gameObject, Hand hand)
    {
        var interactor = hand == Hand.Left ? _leftHandInteractor : _rightHandInteractor;
        if (IsHoldingItem(hand))
        {
            var itemInHand = GetItemInHand(hand);
            var interactable = itemInHand.GetComponent<XRBaseInteractable>();

            var layers = interactable.interactionLayers;
            interactable.interactionLayers = 0;

            StartCoroutine(DisableForOneFrame(interactable, layers));
        }

        _interactionManager.SelectEnter(
            interactor,
            gameObject.GetComponent<IXRSelectInteractable>()
        );
    }

    // Hack to drop the grab zone interactable
    private IEnumerator DisableForOneFrame(XRBaseInteractable interactable, int layers)
    {
        yield return new WaitForSeconds(0.1f);
        interactable.interactionLayers = layers;
    }
}
