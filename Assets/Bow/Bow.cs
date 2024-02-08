using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    float _startBend = 0.3f;

    [SerializeField]
    float _maxPullLength = 0.7f;

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

    private Animator _animator;
    private GrabManager _grabManager;

    private void Start()
    {
        _grabManager = GrabManager.Instance;

        _animator = GetComponent<Animator>();
        _animator.SetFloat("PullAmount", _startBend);
        StartCoroutine(ResetBowStringAndNotch());

        _stringNotch.OnStringReleased += ReleaseString;
        _stringNotch.OnStringNotchedMoved += UpdateString;
    }

    private void OnDestroy()
    {
        _stringNotch.OnStringReleased -= ReleaseString;
        _stringNotch.OnStringNotchedMoved -= UpdateString;
    }

    private IEnumerator ResetBowStringAndNotch()
    {
        yield return new WaitForEndOfFrame();
        ResetStringNotch();
        UpdateString();
    }

    private void UpdateString()
    {
        var topPosition = transform.InverseTransformPoint(_topOfBow.position);
        var bottomPosition = transform.InverseTransformPoint(_bottomOfBow.position);
        var notchPosition = transform.InverseTransformPoint(_stringNotch.transform.position);

        _string.SetPosition(0, topPosition);
        _string.SetPosition(1, notchPosition);
        _string.SetPosition(2, bottomPosition);
    }

    private void ResetStringNotch()
    {
        var topPosition = _topOfBow.position;
        var bottomPosition = _bottomOfBow.position;
        _stringNotch.transform.position = (topPosition + bottomPosition) / 2f;
    }

    private void ReleaseString()
    {
        (float power, Vector3 direction) = CalculatePower();
        _stringNotch.FireArrow(power, direction);
        StartCoroutine(ResetBowStringAndNotch());
        Debug.Log($"Power: {power}      Directon: {direction}");
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
