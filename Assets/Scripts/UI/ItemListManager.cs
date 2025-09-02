using UnityEngine;
using UnityEngine.UI;

public class ItemListManager : MonoBehaviour
{
    [SerializeField] 
    private Transform _contentParent;

    [SerializeField] 
    private GameObject _itemButtonPrefab;

    [SerializeField] 
    private Button _backButton;

    [SerializeField]
    private GameObject _categoryPanel;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackClicked);
    }

    public async void LoadCategoryItems(ApiCategoryType category)
    {
        foreach (Transform child in _contentParent)
        { 
            Destroy(child.gameObject);
        }

        ApiCategoryResource categoryResource = await Gamemanager.Instance.FetchCategory(category);
        if (categoryResource == null) return;

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
    }

}
