using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _text;
    #endregion

    #region Fields
    private ApiCategoryType _category;
    private Action<ApiCategoryType> _onClicked;
    #endregion

    public void Initialize(ApiCategoryType category, Action<ApiCategoryType> onClicked)
    {
        _category = category;
        _text.text = _category.ToString();
        _onClicked = onClicked;
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        _onClicked?.Invoke(_category);
    }
}
