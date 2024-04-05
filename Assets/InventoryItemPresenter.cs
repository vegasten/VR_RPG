using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPresenter : MonoBehaviour
{
    [SerializeField] TMP_Text _countText;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] Image _image;

    public void SetCount(int count)
    {
        _countText.text = $"{count} X";
    }

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    // TODO SetImage

}
