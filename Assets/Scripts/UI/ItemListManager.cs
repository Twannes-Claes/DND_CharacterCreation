using UnityEngine.UI;
using UnityEngine;

public class ItemListManager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] 
    private Transform _contentParent;

    [SerializeField] 
    private GameObject _itemButtonPrefab;

    [SerializeField] 
    private Button _backButton;

    [SerializeField]
    private GameObject _categoryPanel;

    [SerializeField]
    private GameObject _loadIcon;
    #endregion

    #region LifeCycle
    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackClicked);
    }
    #endregion

    #region Functions
    public async void LoadCategoryItems(ApiCategoryType category)
    {
        foreach (Transform child in _contentParent)
        { 
            Destroy(child.gameObject);
        }

        _loadIcon.SetActive(true);

        ApiCategoryResource categoryResource = await Gamemanager.Instance.FetchCategory(category);
        if (categoryResource == null) return;

        _loadIcon.SetActive(false);
        _loadIcon.transform.rotation = Quaternion.identity;

        foreach (ApiResource item in categoryResource.results)
        {
            GameObject itemGO = Instantiate(_itemButtonPrefab, _contentParent);
            ItemButton itemButton = itemGO.GetComponent<ItemButton>();
            itemButton.Initialize(item);
        }
    }

    private void OnBackClicked()
    {
        gameObject.SetActive(false);

        foreach (Transform child in _contentParent)
        {
            Destroy(child.gameObject);
        }

        _categoryPanel.SetActive(true);

        Gamemanager.Instance.DetailPanel.SetTitle(string.Empty);
    }
    #endregion
}
