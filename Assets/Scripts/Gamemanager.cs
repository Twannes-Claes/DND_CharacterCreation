using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class Gamemanager : MonoBehaviour
{
    #region Fields
    private Dictionary<ApiCategoryType, ApiCategoryResource> _cachedCategories = new Dictionary<ApiCategoryType, ApiCategoryResource>();
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
    #endregion  
}

