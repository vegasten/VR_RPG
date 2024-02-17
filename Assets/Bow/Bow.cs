using System;
using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    float _startBend = 0.3f;

    [SerializeField]
    float _maxBend = 1f;

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
        SetAnimatorPullAmount(_startBend);
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
            SetAnimatorPullAmount(_startBend);
            return;
        }

        var limitedNotchPosition = CalculateLimitedStringNotchPosition();
        _stringNotch.UpdateLimitedNotchPosition(limitedNotchPosition);
        UpdateString(limitedNotchPosition);
        UpdateAnimatorBowBend();
    }

    private bool IsAngleTooLarge()
    {
        bool bowInRightHand = _grabManager.IsLayerInRightHand(Layers.Bow);
        var activeBowNotch = bowInRightHand ? _leftBowNotch : _rightBowNotch;
        var bowNotchForward = activeBowNotch.forward;

        var testVector = activeBowNotch.position - _stringNotch.transform.position;

        var angle = Vector3.Angle(bowNotchForward, testVector);

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
        SetAnimatorPullAmount(_startBend);
    }

    private (float, Vector3) CalculatePower()
    {
        var pullData = NormalizedPullData();
        float clampedPullDistance = Mathf.Clamp(pullData.distance, 0.0f, 1.0f);

        var power = clampedPullDistance * _bowPower;

        return (power, pullData.direction);
    }

    private void UpdateAnimatorBowBend()
    {
        float normalizedPullDistance = NormalizedPullData().distance;

        Debug.Log(normalizedPullDistance);
        float clampedPullDistance = Mathf.Clamp(normalizedPullDistance, _startBend, _maxBend);

        SetAnimatorPullAmount(clampedPullDistance);
    }

    private void SetAnimatorPullAmount(float pullAmount)
    {
        _animator.SetFloat("PullAmount", pullAmount);
    }

    private (float distance, Vector3 direction) NormalizedPullData()
    {
        bool bowInRightHand = _grabManager.IsLayerInRightHand(Layers.Bow);
        var activeNotch = bowInRightHand ? _leftBowNotch : _rightBowNotch;

        var pullDirection = (activeNotch.position - _stringNotch.transform.position);
        var idealDirection = activeNotch.forward;

        var normalizedDistance = Vector3.Dot(pullDirection, idealDirection) / _maxPullLength;

        return (normalizedDistance, pullDirection);
    }
}
