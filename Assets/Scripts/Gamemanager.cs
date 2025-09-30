using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

[DefaultExecutionOrder(-100)]
public class Gamemanager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private DetailPanel _detailPanel;

    [SerializeField]
    private List<AbilityScoreInputField> _abilityScores;
    #endregion

    #region Fields
    private readonly Dictionary<ApiCategoryType, ApiCategoryResource> _cachedCategories = new Dictionary<ApiCategoryType, ApiCategoryResource>();

    private readonly Dictionary<AbilityScores, AbilityScoreInputField> _cachedAbilityScores = new Dictionary<AbilityScores, AbilityScoreInputField>();
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

            foreach (AbilityScoreInputField field in _abilityScores)
            {
                _cachedAbilityScores[field.AbilityScore] = field;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Simulate loading the ability scores and then setting the value

        foreach (AbilityScoreInputField field in _cachedAbilityScores.Values)
        {
            if (field != null)
            {
                field.OnAbilityScoreChanged?.Invoke(field.AbilityModifier);
            }
        }
    }
    #endregion

    #region Functions
    public AbilityScoreInputField GetAbilityScore(AbilityScores abilityScore)
    {
        if (_cachedAbilityScores.ContainsKey(abilityScore) == false)
            return null;

        return _cachedAbilityScores[abilityScore];
    }

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

    public void AddAbilityScore(AbilityScoreInputField inputField)
    {
        AbilityScores key = inputField.AbilityScore;

        if (!_cachedAbilityScores.ContainsKey(key))
        {
            _cachedAbilityScores[key] = inputField;
        }
    }

    public void RemoveAbilityScore(AbilityScoreInputField inputField)
    {
        AbilityScores key = inputField.AbilityScore;

        _cachedAbilityScores.Remove(key);
    }
    #endregion  
}

