using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _startBend = 0.3f;
    [SerializeField] float _maxPullLength = 0.5f;

    [Header("Data")]
    [SerializeField] Transform _topOfBow;
    [SerializeField] Transform _bottomOfBow;
    [SerializeField] LineRenderer _string;
    [SerializeField] Transform _bowNotch;
    [SerializeField] StringNotch _stringNotch;

    private Animator _animator;

    private void Start()
    {
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
        //var topPosition = _topOfBow.position - transform.position;
        //var bottomPosition = _bottomOfBow.position - transform.position;
        //var notchPosition = _stringNotch.transform.position - transform.position;


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
        StartCoroutine(ResetBowStringAndNotch());
        Debug.Log($"Power: {power}      Directon: {direction}");
    }

    private (float, Vector3) CalculatePower()
    {
        var pullDirection = (_bowNotch.position - _stringNotch.transform.position);
        var idealDirection = _bowNotch.forward;

        float power = Vector3.Dot(pullDirection, idealDirection) / _maxPullLength;

        return (Mathf.Clamp(power, 0.0f, 1.0f), pullDirection.normalized);
    }
}
