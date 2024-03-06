using TMPro;
using UnityEngine;

public class HandDisplayPresenter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private GameObject _panel;

    // Start is called before the first frame update
    void Start()
    {
        _panel.SetActive(false);
    }

    public void Enable(bool shouldEnable)
    {
        _panel.SetActive(shouldEnable);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}
