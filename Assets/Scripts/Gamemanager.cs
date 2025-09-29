using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Gamemanager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private DetailPanel _detailPanel;
    #endregion

    #region Fields
    private Dictionary<ApiCategoryType, ApiCategoryResource> _cachedCategories = new Dictionary<ApiCategoryType, ApiCategoryResource>();

    private Dictionary<int, CustomInput> _cachedInputFields = new Dictionary<int, CustomInput>();
    #endregion

    #region Properties
    public DetailPanel DetailPanel => _detailPanel;
    #endregion 

    #region Statics
    public static Gamemanager Instance { get; private set; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (CustomInput field in _cachedInputFields.Values)
        {
            if (field != null)
            {
                field.ApplyInputToLinkedText();
            }
        }
    }
    #endregion

    #region Functions
    public async Task<ApiCategoryResource> FetchCategory(ApiCategoryType category)
    {
        if (_cachedCategories.TryGetValue(category, out var resource))
        {
            return resource;
        }

        ApiCategoryResource result = await ApiHelper.FetchCategoryAsync(category); ;
        if (result != null)
        {
            _cachedCategories[category] = result;
        }

        return result;
    }

    public void CacheInputField(CustomInput inputField)
    {
        int key = inputField.GetInstanceID();

        if (!_cachedInputFields.ContainsKey(key))
        {
            _cachedInputFields[key] = inputField;
        }
    }
    #endregion  
}

