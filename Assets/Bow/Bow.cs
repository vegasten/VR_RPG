using System;
using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    float _startBend = 0.3f;

    [SerializeField]
    float _maxPullLength = 0.65f;

    [SerializeField]
    float _bowPower = 20.0f;

    [Header("Data")]
    [SerializeField]
    Transform _topOfBow;

    [SerializeField]
    Transform _bottomOfBow;

    [SerializeField]
    LineRenderer _string;

    [SerializeField]
    Transform _rightBowNotch;

    [SerializeField]
    Transform _leftBowNotch;

    [SerializeField]
    StringNotch _stringNotch;

    [SerializeField]
    float _arrowAngleLimit = 60f;

    private Animator _animator;
    private GrabManager _grabManager;
    private bool _hasArrowInStringNotch = false;

    private void Start()
    {
        _grabManager = GrabManager.Instance;

        _animator = GetComponent<Animator>();
        _animator.SetFloat("PullAmount", _startBend);
        StartCoroutine(ResetBowStringAndNotch());

        _stringNotch.OnStringReleased += ReleaseString;
        _stringNotch.OnArrowSet += OnArrowSet;
    }

    private void OnDestroy()
    {
        _stringNotch.OnStringReleased -= ReleaseString;
        _stringNotch.OnArrowSet -= OnArrowSet;
    }

    private void OnArrowSet()
    {
        _hasArrowInStringNotch = true;
    }

    private void Update()
    {
        if (!_hasArrowInStringNotch)
            return;

        if (IsAngleTooLarge())
        {
            _hasArrowInStringNotch = false;
            StartCoroutine(ResetBowStringAndNotch());
            _stringNotch.UnstringArrow();
            return;
        }

        var limitedNotchPosition = CalculateLimitedStringNotchPosition();

        _stringNotch.UpdateLimitedNotchPosition(limitedNotchPosition);
        UpdateString(limitedNotchPosition);
    }

    private bool IsAngleTooLarge()
    {
        bool bowInRightHand = _grabManager.IsLayerInRightHand(Layers.Bow);
        var activeBowNotch = bowInRightHand ? _leftBowNotch : _rightBowNotch;
        var bowNotchForward = activeBowNotch.forward;

        var testVector = activeBowNotch.position - _stringNotch.transform.position;

        var angle = Vector3.Angle(bowNotchForward, testVector);

        Debug.Log(angle);

        return angle > _arrowAngleLimit;
    }

    private IEnumerator ResetBowStringAndNotch()
    {
        yield return new WaitForEndOfFrame();
        ResetStringNotch();
    }

    private void UpdateString(Vector3 notchPosition)
    {
        var localTopPosition = transform.InverseTransformPoint(_topOfBow.position);
        var localBottomPosition = transform.InverseTransformPoint(_bottomOfBow.position);
        var localNotchPosition = transform.InverseTransformPoint(notchPosition);

        _string.SetPosition(0, localTopPosition);
        _string.SetPosition(1, localNotchPosition);
        _string.SetPosition(2, localBottomPosition);
    }

    private Vector3 CalculateLimitedStringNotchPosition()
    {
        bool bowInRightHand = _grabManager.IsLayerInRightHand(Layers.Bow);
        var activeBowNotch = bowInRightHand ? _leftBowNotch : _rightBowNotch;

        var actualStringNotchPosition = _stringNotch.transform.position;
        var distance = Vector3.Distance(actualStringNotchPosition, activeBowNotch.position);

        if (distance <= _maxPullLength)
            return _stringNotch.transform.position;

        var direction = (actualStringNotchPosition - activeBowNotch.position).normalized;

        return activeBowNotch.position + direction * _maxPullLength;
    }

    private void ResetStringNotch()
    {
        var topPosition = _topOfBow.position;
        var bottomPosition = _bottomOfBow.position;
        _stringNotch.transform.position = (topPosition + bottomPosition) / 2f;
        UpdateString(_stringNotch.transform.position);
    }

    private void ReleaseString()
    {
        (float power, Vector3 direction) = CalculatePower();
        _stringNotch.FireArrow(power, direction);
        StartCoroutine(ResetBowStringAndNotch());
        _hasArrowInStringNotch = false;
    }

    private (float, Vector3) CalculatePower()
    {
        bool bowInRightHand = _grabManager.IsLayerInRightHand(Layers.Bow);
        var activeNotch = bowInRightHand ? _leftBowNotch : _rightBowNotch;

        var pullDirection = (activeNotch.position - _stringNotch.transform.position);
        var idealDirection = activeNotch.forward;

        float powerMultiplier = Vector3.Dot(pullDirection, idealDirection) / _maxPullLength;
        float clampedPowerMultiplier = Mathf.Clamp(powerMultiplier, 0.0f, 1.0f);

        return (clampedPowerMultiplier * _bowPower, pullDirection.normalized);
    }
}
