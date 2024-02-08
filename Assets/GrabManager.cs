using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabManager : MonoBehaviour
{
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

    public enum Hand
    {
        Right,
        Left
    };

    public XRDirectInteractor _leftHandInteractor;
    public XRDirectInteractor _rightHandInteractor;

    public bool IsHoldingItem(Hand hand)
    {
        if (hand == Hand.Left)
            return _leftHandInteractor.hasSelection;
        else
            return _rightHandInteractor.hasSelection;
    }

    public GameObject? GetItemInHand(Hand hand)
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

    internal bool IsGameobjectHeld(GameObject gameObject)
    {
        var itemInRight = GetItemInHand(Hand.Right);
        var itemInLeft = GetItemInHand(Hand.Left);

        Debug.Log($"Item in right: {itemInRight}");
        Debug.Log($"Item in left: {itemInLeft}");
        Debug.Log($"Item: {gameObject}");
        Debug.Log($"ref right: {ReferenceEquals(gameObject, itemInRight)}");
        Debug.Log($"ref left: {ReferenceEquals(gameObject, itemInLeft)}");

        return (
            ReferenceEquals(gameObject, itemInRight) || ReferenceEquals(gameObject, itemInLeft)
        );
    }
}
