using UnityEngine.UI;
using UnityEngine;
using TMPro;

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

    #region LifeCycle
    public void OnEnable()
    {
        _button.onClick.AddListener(OnClicked);
    }

    public void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    #endregion

    #region Functions
    public void Initialize(ApiResource itemResource)
    {
        _itemResource = itemResource;
        _text.SetText(_itemResource.name);
    }

    private void OnClicked()
    {
        //GameManager.Instance.DetailPanel.SetInfo(_itemResource.name);
        //GameManager.Instance.AddToApiLinks(_itemResource);
    }
    #endregion
}
