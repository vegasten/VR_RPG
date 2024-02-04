using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] Transform _topOfBow;
    [SerializeField] Transform _bottomOfBow;
    [SerializeField] LineRenderer _string;
    [SerializeField] Transform _stringNotch;
    [SerializeField] Transform _bowNotch;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("PullAmount", 0.3f); ;
        StartCoroutine(InitializeBow());

        //StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        while (true)
        {
            _animator.SetFloat("PullAmount", 0.0f);
            yield return new WaitForEndOfFrame();
            ResetStringNotch();
            UpdateString();

            yield return new WaitForSeconds(1f);

            _animator.SetFloat("PullAmount", 0.5f);
            yield return new WaitForEndOfFrame();
            ResetStringNotch();
            UpdateString();

            yield return new WaitForSeconds(1f);

            _animator.SetFloat("PullAmount", 1.0f);
            yield return new WaitForEndOfFrame();
            ResetStringNotch();
            UpdateString();

            yield return new WaitForSeconds(1f);

        }
    }

    private IEnumerator InitializeBow()
    {
        yield return new WaitForEndOfFrame();
        ResetStringNotch();
        UpdateString();
    }

    private void UpdateString()
    {
        var topPosition = _topOfBow.position - transform.position;
        var bottomPosition = _bottomOfBow.position - transform.position;
        var notchPosition = _stringNotch.position - transform.position;

        _string.SetPosition(0, topPosition);
        _string.SetPosition(1, notchPosition);
        _string.SetPosition(2, bottomPosition);
    }

    private void ResetStringNotch()
    {
        var topPosition = _topOfBow.position;
        var bottomPosition = _bottomOfBow.position;
        _stringNotch.position = (topPosition + bottomPosition) / 2f;
    }
}
