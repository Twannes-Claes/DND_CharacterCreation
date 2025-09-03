using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

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

    #region LifeCycle
    private void OnEnable()
    {
        _button.onClick.AddListener(OnClicked);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    #endregion

    #region Functions
    public void Initialize(ApiCategoryType category, Action<ApiCategoryType> onClicked)
    {
        _category = category;
        _text.text = ApiHelper.CategoryToString(category);
        _onClicked = onClicked;
    }

    private void OnClicked()
    {
        _onClicked?.Invoke(_category);
    }
    #endregion
}   
