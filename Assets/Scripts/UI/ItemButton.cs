using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _text;
    #endregion

    #region Fields
    private ApiResource _itemResource;
    #endregion

    public void Initialize(ApiResource itemResource)
    {
        _itemResource = itemResource;
        _text.SetText(_itemResource.name);
        _button.onClick.AddListener(OnClicked);
    }

    public void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void OnClicked()
    {
        Debug.Log($"Clicked item {_itemResource.index}");
    }
}
