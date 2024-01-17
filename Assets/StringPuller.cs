using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StringPuller : XRBaseInteractable
{
    public static event Action<float> OnStringReleased;

    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private GameObject _notch;
    [SerializeField] private LineRenderer _lineRenderer;

    public float PullAmount = 0.0f;
    private IXRSelectInteractor _interactor;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {

        _interactor = args.interactorObject;
    }

    public void Release()
    {
        OnStringReleased?.Invoke(PullAmount);
        _interactor = null;
        PullAmount = 0.0f;
        _notch.transform.localPosition = new Vector3(_notch.transform.localPosition.x, 0f, _notch.transform.localPosition.z);
        UpdateString();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                Vector3 pullPosition = _interactor.transform.position;
                PullAmount = CalculatePullAmount(pullPosition);

                UpdateString();
            }
        }
    }

    private float CalculatePullAmount(Vector3 pullPosition)
    {
        var pullDirection = pullPosition - _start.position;
        var targetDirection = _end.position - _start.position;

        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;

        return Mathf.Clamp(pullValue, 0.0f, 1.0f);
    }

    private void UpdateString()
    {
        var linePosition = Vector3.up * Mathf.Lerp(_start.localPosition.y, _end.localPosition.y, PullAmount);
        _notch.transform.localPosition = new Vector3(_notch.transform.localPosition.x, linePosition.y, _notch.transform.localPosition.z);
        _lineRenderer.SetPosition(1, linePosition);
    }
}
