using UnityEngine;
using TMPro;

public class DetailPanel : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private TextMeshProUGUI _titleText;
    #endregion

    public void SetTitle(string text)
    {
        _titleText.SetText(text);
    }
}
