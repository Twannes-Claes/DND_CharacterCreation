using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryListManager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] 
    private RectTransform _contentParent;

    [SerializeField] 
    private GameObject _buttonPrefab;

    [SerializeField]
    private GameObject _itemsPanel;

    [SerializeField] 
    private ItemListManager _itemsListManager;
    #endregion

    private void Awake()
    {
        CreateCategoryButtons();
    }

    private void CreateCategoryButtons()
    {
        foreach (ApiCategoryType category in System.Enum.GetValues(typeof(ApiCategoryType)))
        {
            GameObject buttonGO = Instantiate(_buttonPrefab, _contentParent);
            CategoryButton categoryButton = buttonGO.GetComponent<CategoryButton>();
            categoryButton.Initialize(category, OnCategoryPressed);
        }
    }

    private void OnCategoryPressed(ApiCategoryType category)
    {
        gameObject.SetActive(false);

        _itemsPanel.SetActive(true);
        _itemsListManager.LoadCategoryItems(category);
    }
}
