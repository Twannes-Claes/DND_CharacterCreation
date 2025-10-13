using UnityEngine;
using TMPro;

public class DetailPanel : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private TextMeshProUGUI _titleText;
    #endregion

    public void SetInfo(string text)
    {
        _titleText.SetText(text);
    }
}
